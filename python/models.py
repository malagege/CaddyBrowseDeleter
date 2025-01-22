# from app import db

# class User(db.Model):
#     id = db.Column(db.Integer, primary_key=True)
#     name = db.Column(db.String(80), nullable=False)
#     to_do_delete_files = db.relationship('ToDoDeleteFile', secondary='to_do_delete_file_user', back_populates='users')

# class ToDoDeleteFile(db.Model):
#     id = db.Column(db.Integer, primary_key=True)
#     file_path = db.Column(db.String(200), nullable=False)
#     dir_path = db.Column(db.String(200), nullable=False)
#     is_ready_to_delete = db.Column(db.Boolean, default=False)
#     users = db.relationship('User', secondary='to_do_delete_file_user', back_populates='to_do_delete_files')

# class ToDoDeleteFileUser(db.Model):
#     __tablename__ = 'to_do_delete_file_user'
#     to_do_delete_files_id = db.Column(db.Integer, db.ForeignKey('to_do_delete_file.id'), primary_key=True)
#     users_id = db.Column(db.Integer, db.ForeignKey('user.id'), primary_key=True)