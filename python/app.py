from flask import Flask, request, jsonify
from flask_sqlalchemy import SQLAlchemy
from flask_migrate import Migrate
from flask_apscheduler import APScheduler
import os
import logging
import shutil
import json

# app = Flask(__name__)
# app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///db/app.db'
# app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False

app = Flask(__name__)
db_path = os.path.join(os.getcwd(), 'db', 'app.db')
app.config['SQLALCHEMY_DATABASE_URI'] = f'sqlite:///{db_path}'
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False

db = SQLAlchemy(app)
migrate = Migrate(app, db)
scheduler = APScheduler()
scheduler.init_app(app)
scheduler.start()

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(80), nullable=False)
    to_do_delete_files = db.relationship('ToDoDeleteFile', secondary='to_do_delete_file_user', back_populates='users')

class ToDoDeleteFile(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    file_path = db.Column(db.String(200), nullable=False)
    dir_path = db.Column(db.String(200), nullable=False)
    is_ready_to_delete = db.Column(db.Boolean, default=False)
    users = db.relationship('User', secondary='to_do_delete_file_user', back_populates='to_do_delete_files')
    def to_dict(self):
        return {
            'filePath': self.file_path,
            'dirPath': self.dir_path,
            'users': [{'name': user.name} for user in self.users],
            'isReadyToDelete': self.is_ready_to_delete
        }

class ToDoDeleteFileUser(db.Model):
    __tablename__ = 'to_do_delete_file_user'
    to_do_delete_files_id = db.Column(db.Integer, db.ForeignKey('to_do_delete_file.id'), primary_key=True)
    users_id = db.Column(db.Integer, db.ForeignKey('user.id'), primary_key=True)

@app.route('/api/ToDoDeleteFile', methods=['GET'])
def get_to_do_delete_files():
    path = request.args.get('path')
    all_users = User.query.all()
    files_query = ToDoDeleteFile.query.options(db.joinedload(ToDoDeleteFile.users))
    if path:
        filtered_files = files_query.filter(ToDoDeleteFile.file_path.like(f'{path}%')).all()
        for file in filtered_files:
            file.is_ready_to_delete = all(user in file.users for user in all_users)
        return json.dumps([file.to_dict() for file in filtered_files], ensure_ascii=False)
    files = files_query.all()
    for file in files:
        file.is_ready_to_delete = all(user in file.users for user in all_users)
    return json.dumps([file.to_dict() for file in files], ensure_ascii=False)

@app.route('/api/ToDoDeleteFile', methods=['POST'])
def post_to_do_delete_file():
    data = request.json
    user = User.query.filter_by(name=data['userName']).first()
    if not user:
        return jsonify({'error': '使用者不存在'}), 400
    to_do_delete_file = ToDoDeleteFile.query.filter_by(file_path=data['filePath']).first()
    if to_do_delete_file and user in to_do_delete_file.users:
        to_do_delete_file.users.remove(user)
    elif to_do_delete_file:
        to_do_delete_file.users.append(user)
    else:
        dir_path = os.path.dirname(data['filePath']).replace("\\", "/")
        if not dir_path:
            return jsonify({'error': '路徑錯誤'}), 400
        to_do_delete_file = ToDoDeleteFile(file_path=data['filePath'], dir_path=dir_path, users=[user])
        db.session.add(to_do_delete_file)
    db.session.commit()
    return jsonify({'success': True})

@app.route('/api/ToDoDeleteFile/execute', methods=['GET'])
def run_delete_files_job():
    delete_files_job()
    return jsonify({'message': '排程已執行'})

def delete_files_job():
    current_directory = os.getcwd()
    logger.info(f'Current Directory: {current_directory}')
    all_users = User.query.all()
    all_users_id = [user.id for user in all_users]
    files_to_delete = ToDoDeleteFile.query.options(db.joinedload(ToDoDeleteFile.users)).all()
    # 確認所有使用者都在 all_users_id 中
    files_to_delete = [file for file in files_to_delete if all(user.id in all_users_id for user in file.users) and len(file.users) == len(all_users_id)]

    prefix_path = './extHDD'
    for file in files_to_delete:
        # file_path = os.path.join(prefix_path, file.file_path)
        file_path = prefix_path + file.file_path
        if not os.path.exists(file_path):
            db.session.delete(file)
            logger.warning(f'File not found: {file.file_path}')
            continue
        logger.info(f'Deleting: {file.file_path}')
        if os.path.isdir(file_path):
            logger.info(f'Deleting directory: {file.file_path}')
            shutil.rmtree(file_path)
        else:
            logger.info(f'Deleting file: {file.file_path}')
            os.remove(file_path)
        db.session.delete(file)
    db.session.commit()

scheduler.add_job(id='delete_files_job', func=delete_files_job, trigger='cron', hour=19)

def insert_default_users():
    logger.info('Inserting default users')
    if not User.query.filter_by(name='malagege').first():
        user1 = User(name='malagege')
        db.session.add(user1)
    if not User.query.filter_by(name='chevy').first():
        user2 = User(name='chevy')
        db.session.add(user2)
    db.session.commit()

@app.before_first_request
def before_first_request():
    insert_default_users()

if __name__ == '__main__':
    app.run()