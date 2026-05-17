import logging
from flask import Flask
from .routes import main_bp

def create_app():
    app = Flask(__name__)
    
    # Configure Logging
    logging.basicConfig(
        level=logging.INFO,
        format='%(levelname)s %(asctime)s %(module)s %(message)s'
    )
    
    # Register Blueprints
    app.register_blueprint(main_bp)
    
    return app
