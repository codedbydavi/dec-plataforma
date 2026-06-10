import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router';
import { useApp } from '../context/AppContext';
import { Header } from './Header';
import { Button } from './ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './ui/card';
import { Badge } from './ui/badge';
import { Progress } from './ui/progress';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from './ui/table';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer, PieChart, Pie, Cell } from 'recharts';
import { GraduationCap, Users, Target, TrendingUp, Award, Plus } from 'lucide-react';

interface Student {
  id: string;
  name: string;
  email: string;
  challengesCompleted: number;
  averageScore: number;
  lastActivity: string;
  familyScenarios: number;
  totalSavings: number;
}

export function TeacherDashboard() {
  const { user, challenges } = useApp();
  const navigate = useNavigate();

  const [students] = useState<Student[]>([
    {
      id: '1',
      name: 'Ana Silva',
      email: 'ana.silva@istec.pt',
      challengesCompleted: 3,
      averageScore: 85,
      lastActivity: '2026-03-29',
      familyScenarios: 2,
      totalSavings: 1500,
    },
    {
      id: '2',
      name: 'Bruno Costa',
      email: 'bruno.costa@istec.pt',
      challengesCompleted: 2,
      averageScore: 78,
      lastActivity: '2026-03-28',
      familyScenarios: 1,
      totalSavings: 800,
    },
    {
      id: '3',
      name: 'Carla Mendes',
      email: 'carla.mendes@istec.pt',
      challengesCompleted: 4,
      averageScore: 92,
      lastActivity: '2026-03-30',
      familyScenarios: 3,
      totalSavings: 2200,
    },
    {
      id: '4',
      name: 'Daniel Rodrigues',
      email: 'daniel.rodrigues@istec.pt',
      challengesCompleted: 1,
      averageScore: 65,
      lastActivity: '2026-03-25',
      familyScenarios: 1,
      totalSavings: 400,
    },
    {
      id: '5',
      name: 'Eva Sousa',
      email: 'eva.sousa@istec.pt',
      challengesCompleted: 3,
      averageScore: 88,
      lastActivity: '2026-03-30',
      familyScenarios: 2,
      totalSavings: 1800,
    },
  ]);

  useEffect(() => {
    if (!user || user.role !== 'teacher') {
      navigate('/login');
    }
  }, [user, navigate]);

  if (!user || user.role !== 'teacher') {
    return null;
  }

  const totalChallenges = challenges.length;
  const activeChallenges = challenges.filter(c => !c.completed).length;
  const avgCompletionRate = students.reduce((sum, s) => sum + (s.challengesCompleted / totalChallenges) * 100, 0) / students.length;

  // Mock data for charts
  const studentPerformance = students.map(s => ({
    name: s.name.split(' ')[0],
    desafios: s.challengesCompleted,
    pontuacao: s.averageScore,
  }));

  const challengeCompletion = [
    { name: 'Concluídos', value: challenges.filter(c => c.completed).length, color: '#0C73B7' },
    { name: 'Em Andamento', value: activeChallenges, color: '#2da7df' },
  ];

  return (
    <div className="min-h-screen bg-gray-50">
      <Header />
      
      <div className="max-w-7xl mx-auto p-6">
        <div className="flex items-center justify-between mb-6">
          <div>
            <h1 className="text-3xl mb-2">Dashboard do Professor</h1>
            <p className="text-muted-foreground">
              Gere turmas, desafios e acompanha o progresso dos alunos
            </p>
          </div>
          <Button onClick={() => navigate('/challenges')}>
            <Plus className="w-4 h-4 mr-2" />
            Criar Desafio
          </Button>
        </div>

        {/* Summary Cards */}
        <div className="grid md:grid-cols-4 gap-4 mb-8">
          <Card>
            <CardHeader className="pb-2">
              <CardDescription>Total de Alunos</CardDescription>
              <CardTitle className="text-3xl flex items-center gap-2">
                <Users className="w-6 h-6 text-primary" />
                {students.length}
              </CardTitle>
            </CardHeader>
          </Card>

          <Card>
            <CardHeader className="pb-2">
              <CardDescription>Desafios Criados</CardDescription>
              <CardTitle className="text-3xl flex items-center gap-2">
                <Target className="w-6 h-6 text-secondary" />
                {totalChallenges}
              </CardTitle>
            </CardHeader>
          </Card>

          <Card>
            <CardHeader className="pb-2">
              <CardDescription>Taxa Média de Conclusão</CardDescription>
              <CardTitle className="text-3xl flex items-center gap-2">
                <TrendingUp className="w-6 h-6 text-green-500" />
                {avgCompletionRate.toFixed(0)}%
              </CardTitle>
            </CardHeader>
          </Card>

          <Card>
            <CardHeader className="pb-2">
              <CardDescription>Pontuação Média</CardDescription>
              <CardTitle className="text-3xl flex items-center gap-2">
                <Award className="w-6 h-6 text-yellow-500" />
                {(students.reduce((sum, s) => sum + s.averageScore, 0) / students.length).toFixed(0)}
              </CardTitle>
            </CardHeader>
          </Card>
        </div>

        {/* Charts */}
        <div className="grid md:grid-cols-2 gap-6 mb-8">
          <Card>
            <CardHeader>
              <CardTitle>Desempenho dos Alunos</CardTitle>
              <CardDescription>Desafios concluídos e pontuação média</CardDescription>
            </CardHeader>
            <CardContent>
              <ResponsiveContainer width="100%" height={300}>
                <BarChart data={studentPerformance}>
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis dataKey="name" />
                  <YAxis />
                  <Tooltip />
                  <Legend />
                  <Bar dataKey="desafios" name="Desafios Concluídos" fill="#0C73B7" />
                  <Bar dataKey="pontuacao" name="Pontuação Média" fill="#2da7df" />
                </BarChart>
              </ResponsiveContainer>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle>Estado dos Desafios</CardTitle>
              <CardDescription>Desafios concluídos vs. em andamento</CardDescription>
            </CardHeader>
            <CardContent className="flex items-center justify-center">
              <ResponsiveContainer width="100%" height={300}>
                <PieChart>
                  <Pie
                    data={challengeCompletion}
                    cx="50%"
                    cy="50%"
                    labelLine={false}
                    label={({ name, value }) => `${name}: ${value}`}
                    outerRadius={100}
                    fill="#8884d8"
                    dataKey="value"
                  >
                    {challengeCompletion.map((entry, index) => (
                      <Cell key={`cell-${index}`} fill={entry.color} />
                    ))}
                  </Pie>
                  <Tooltip />
                </PieChart>
              </ResponsiveContainer>
            </CardContent>
          </Card>
        </div>

        {/* Students Table */}
        <Card>
          <CardHeader>
            <div className="flex items-center justify-between">
              <div>
                <CardTitle>Turma - Literacia Financeira 2025/2026</CardTitle>
                <CardDescription>
                  Lista de alunos e progresso individual
                </CardDescription>
              </div>
            </div>
          </CardHeader>
          <CardContent>
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Aluno</TableHead>
                  <TableHead>Email</TableHead>
                  <TableHead className="text-center">Cenários</TableHead>
                  <TableHead className="text-center">Desafios</TableHead>
                  <TableHead className="text-right">Poupança Total</TableHead>
                  <TableHead className="text-center">Pontuação</TableHead>
                  <TableHead>Última Atividade</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {students.map((student) => (
                  <TableRow key={student.id}>
                    <TableCell className="font-medium">{student.name}</TableCell>
                    <TableCell className="text-muted-foreground">{student.email}</TableCell>
                    <TableCell className="text-center">{student.familyScenarios}</TableCell>
                    <TableCell className="text-center">
                      <Badge variant="secondary">
                        {student.challengesCompleted}/{totalChallenges}
                      </Badge>
                    </TableCell>
                    <TableCell className="text-right font-medium">€{student.totalSavings}</TableCell>
                    <TableCell className="text-center">
                      <div className="flex flex-col items-center gap-1">
                        <span className={`font-semibold ${
                          student.averageScore >= 80 ? 'text-green-600' : 
                          student.averageScore >= 60 ? 'text-yellow-600' : 
                          'text-red-600'
                        }`}>
                          {student.averageScore}
                        </span>
                        <Progress value={student.averageScore} className="h-1 w-16" />
                      </div>
                    </TableCell>
                    <TableCell className="text-sm text-muted-foreground">
                      {new Date(student.lastActivity).toLocaleDateString('pt-PT')}
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </CardContent>
        </Card>

        {/* Quick Actions */}
        <div className="mt-8 flex flex-wrap gap-4">
          <Button onClick={() => navigate('/challenges')}>
            <Target className="w-4 h-4 mr-2" />
            Gerir Desafios
          </Button>
          <Button variant="outline">
            <GraduationCap className="w-4 h-4 mr-2" />
            Ver Submissões
          </Button>
          <Button variant="outline">
            <Users className="w-4 h-4 mr-2" />
            Gerir Turmas
          </Button>
        </div>
      </div>
    </div>
  );
}
