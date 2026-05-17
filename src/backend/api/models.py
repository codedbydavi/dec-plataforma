from django.db import models
from django.contrib.auth.models import AbstractUser
import uuid

# --- Lookup Tables ---

class Role(models.Model):
    role = models.CharField(max_length=50, unique=True)
    def __str__(self): return self.role
    class Meta:
        verbose_name = "Role"
        verbose_name_plural = "Roles"

class UserStatus(models.Model):
    status = models.CharField(max_length=50, unique=True)
    def __str__(self): return self.status
    class Meta:
        verbose_name = "User Status"
        verbose_name_plural = "User Statuses"

class ClassStatus(models.Model):
    status = models.CharField(max_length=50, unique=True)
    def __str__(self): return self.status
    class Meta:
        verbose_name = "Class Status"
        verbose_name_plural = "Class Statuses"

class Gender(models.Model):
    name = models.CharField(max_length=50, unique=True)
    def __str__(self): return self.name
    class Meta:
        verbose_name = "Gender"
        verbose_name_plural = "Genders"

# --- Users ---

class CustomUser(AbstractUser):
    name = models.CharField(max_length=255, blank=True)
    gender = models.ForeignKey(Gender, on_delete=models.SET_NULL, null=True, blank=True)
    birth_date = models.DateField(null=True, blank=True)
    img_url = models.URLField(max_length=500, null=True, blank=True)
    user_status = models.ForeignKey(UserStatus, on_delete=models.SET_NULL, null=True, blank=True)
    role_entity = models.ForeignKey(Role, on_delete=models.SET_NULL, null=True, blank=True)
    
    ROLE_CHOICES = (
        ('ADMIN', 'Admin'),
        ('TEACHER', 'Teacher'),
        ('STUDENT', 'Student'),
    )
    role = models.CharField(max_length=10, choices=ROLE_CHOICES, default='STUDENT', db_index=True)

    def __str__(self):
        return f"{self.username} ({self.role})"
    
    class Meta:
        verbose_name = "User"
        verbose_name_plural = "Users"

# --- Education (Classes) ---

class ClassGroup(models.Model):
    name = models.CharField(max_length=100)
    teacher = models.ForeignKey(CustomUser, on_delete=models.CASCADE, related_name='managed_classes')
    join_code = models.CharField(max_length=10, unique=True, db_index=True)
    class_status = models.ForeignKey(ClassStatus, on_delete=models.SET_NULL, null=True, blank=True)
    created_at = models.DateTimeField(auto_now_add=True)

    def save(self, *args, **kwargs):
        if not self.join_code:
            self.join_code = f"DEC-{uuid.uuid4().hex[:6].upper()}"
        super().save(*args, **kwargs)

    def __str__(self):
        return f"{self.name} ({self.join_code})"
    
    class Meta:
        verbose_name = "Class"
        verbose_name_plural = "Classes"
        db_table = 'classes'

class Enrollment(models.Model):
    student = models.ForeignKey(CustomUser, on_delete=models.CASCADE, related_name='enrollments')
    class_group = models.ForeignKey(ClassGroup, on_delete=models.CASCADE, related_name='students')
    enrolled_at = models.DateTimeField(auto_now_add=True)

    class Meta:
        unique_together = ('student', 'class_group')
        verbose_name = 'Enrollment'
        verbose_name_plural = 'Enrollments'

# --- Scenarios and Simulations (Core Logic) ---

class Scenario(models.Model):
    student = models.ForeignKey(CustomUser, on_delete=models.CASCADE, related_name='scenarios')
    family_name = models.CharField(max_length=100)
    initial_balance = models.DecimalField(max_digits=12, decimal_places=2, default=0.0)
    created_at = models.DateTimeField(auto_now_add=True, db_index=True)

    def __str__(self):
        return f"{self.family_name} - {self.student.username}"
    
    class Meta:
        verbose_name = "Scenario"
        verbose_name_plural = "Scenarios"

class Entry(models.Model):
    ENTRY_TYPES = (('INCOME', 'Income'), ('EXPENSE', 'Expense'))
    
    scenario = models.ForeignKey(Scenario, on_delete=models.CASCADE, related_name='entries')
    type = models.CharField(max_length=10, choices=ENTRY_TYPES)
    category = models.CharField(max_length=100)
    amount = models.DecimalField(max_digits=12, decimal_places=2)
    month = models.IntegerField()  # 1-12
    recurrence = models.BooleanField(default=False)

    def __str__(self):
        return f"{self.type}: {self.category} ({self.amount})"
    
    class Meta:
        verbose_name = "Financial Entry"
        verbose_name_plural = "Financial Entries"

class Objective(models.Model):
    scenario = models.ForeignKey(Scenario, on_delete=models.CASCADE, related_name='objectives')
    description = models.CharField(max_length=255)
    target_value = models.DecimalField(max_digits=12, decimal_places=2)
    term_months = models.IntegerField()

    def __str__(self):
        return self.description
    
    class Meta:
        verbose_name = "Savings Objective"
        verbose_name_plural = "Savings Objectives"

class SimulationHistory(models.Model):
    scenario = models.ForeignKey(Scenario, on_delete=models.CASCADE, related_name='histories')
    execution_date = models.DateTimeField(auto_now_add=True)
    json_results = models.JSONField()

    def __str__(self):
        return f"Sim: {self.scenario.family_name} @ {self.execution_date}"
    
    class Meta:
        verbose_name = "Simulation History"
        verbose_name_plural = "Simulation Histories"
