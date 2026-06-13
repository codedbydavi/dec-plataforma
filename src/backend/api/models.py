from django.db import models

# Financial Calculation Engine Models
# This microservice is now stateless or only stores calculation logs.
# Education data (Users, Classes, Scenarios) has moved to the .NET MVC Portal.

class CalculationLog(models.Model):
    request_data = models.JSONField()
    response_data = models.JSONField()
    created_at = models.DateTimeField(auto_now_add=True)

    def __str__(self):
        return f"Calc at {self.created_at}"
