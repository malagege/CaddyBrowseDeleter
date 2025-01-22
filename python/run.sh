#!/bin/bash
export FLASK_APP=app.py
flask db init
# flask db migrate -m 'DB init'
flask db migrate
flask db upgrade
flask run --host=0.0.0.0