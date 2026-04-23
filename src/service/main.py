from flask import Flask, jsonify, request

app = Flask(__name__)

@app.route('/calculate', methods=['POST'])
def calculate():
    # Example logic for the Financial Engine
    data = request.get_json()
    
    # Financial calculation simulation
    income = data.get('income', 0)
    expenses = data.get('expenses', 0)
    savings = income - expenses
    
    return jsonify({
        "status": "success",
        "result": {
            "savings": savings,
            "message": "Calculation performed by the Financial Engine (Flask)"
        }
    })

@app.route('/health', methods=['GET'])
def health():
    return jsonify({"status": "healthy", "service": "Python Microservice"})

if __name__ == '__main__':
    # In Docker, the host must be 0.0.0.0
    app.run(host='0.0.0.0', port=5000)
