from flask import Blueprint, jsonify, request
from .services import FinancialService

main_bp = Blueprint('main', __name__)
financial_service = FinancialService()

@main_bp.route('/calculate', methods=['POST'])
def calculate():
    data = request.get_json()
    if not data:
        return jsonify({"status": "error", "message": "No data provided"}), 400
        
    result = financial_service.calculate_savings(data)
    return jsonify({
        "status": "success",
        "result": result
    })

@main_bp.route('/health', methods=['GET'])
def health():
    return jsonify({
        "status": "healthy",
        "service": "Financial Engine",
        "version": "1.0.0"
    })
