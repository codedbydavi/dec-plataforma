from rest_framework import serializers
from rest_framework_simplejwt.serializers import TokenObtainPairSerializer
from .models import (
    CustomUser, ClassGroup, Enrollment, Scenario, Entry, Objective, SimulationHistory
)

class CustomTokenObtainPairSerializer(TokenObtainPairSerializer):
    @classmethod
    def get_token(cls, user):
        token = super().get_token(user)
        token['role'] = user.role
        token['username'] = user.username
        return token

    def validate(self, attrs):
        data = super().validate(attrs)
        data['role'] = self.user.role
        data['username'] = self.user.username
        return data

class UserSerializer(serializers.ModelSerializer):
    class Meta:
        model = CustomUser
        fields = ('id', 'username', 'email', 'role', 'name', 'gender', 'birth_date', 'img_url')

class ClassGroupSerializer(serializers.ModelSerializer):
    teacher_name = serializers.ReadOnlyField(source='teacher.username')
    student_count = serializers.SerializerMethodField()

    class Meta:
        model = ClassGroup
        fields = ('id', 'name', 'teacher', 'teacher_name', 'join_code', 'student_count', 'created_at', 'class_status')
        read_only_fields = ('teacher', 'join_code')

    def get_student_count(self, obj):
        return obj.students.count()

class EnrollmentSerializer(serializers.ModelSerializer):
    class_name = serializers.ReadOnlyField(source='class_group.name')
    join_code = serializers.CharField(write_only=True)

    class Meta:
        model = Enrollment
        fields = ('id', 'class_group', 'class_name', 'join_code', 'enrolled_at')
        read_only_fields = ('class_group', 'enrolled_at')

    def validate_join_code(self, value):
        try:
            class_group = ClassGroup.objects.get(join_code=value)
            self.context['class_group'] = class_group
        except ClassGroup.DoesNotExist:
            raise serializers.ValidationError("Invalid join code.")
        return value

class EntrySerializer(serializers.ModelSerializer):
    class Meta:
        model = Entry
        fields = '__all__'

class ObjectiveSerializer(serializers.ModelSerializer):
    class Meta:
        model = Objective
        fields = '__all__'

class SimulationHistorySerializer(serializers.ModelSerializer):
    class Meta:
        model = SimulationHistory
        fields = '__all__'

class ScenarioSerializer(serializers.ModelSerializer):
    entries = EntrySerializer(many=True, read_only=True)
    objectives = ObjectiveSerializer(many=True, read_only=True)
    histories = SimulationHistorySerializer(many=True, read_only=True)

    class Meta:
        model = Scenario
        fields = ('id', 'student', 'family_name', 'initial_balance', 'created_at', 'entries', 'objectives', 'histories')
        read_only_fields = ('student',)
