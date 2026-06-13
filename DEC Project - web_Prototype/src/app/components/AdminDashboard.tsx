import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router';
import { useApp } from '../context/AppContext';
import { Header } from './Header';
import { Button } from './ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './ui/card';
import { Badge } from './ui/badge';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from './ui/table';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { Users, GraduationCap, Database, Activity, Shield, CheckCircle, AlertCircle } from 'lucide-react';

interface SystemUser {
  id: string;
  name: string;
  email: string;
  role: 'student' | 'teacher' | 'admin';
  status: 'active' | 'inactive';
  lastLogin: string;
  createdAt: string;
}

export function AdminDashboard() {
  const { user } = useApp();
  const navigate = useNavigate();

  const [systemUsers] = useState<SystemUser[]>([
    { id: '1', name: 'Ana Silva', email: 'ana.silva@istec.pt', role: 'student', status: 'active', lastLogin: '2026-03-30', createdAt: '2026-01-15' },
    { id: '2', name: 'Bruno Costa', email: 'bruno.costa@istec.pt', role: 'student', status: 'active', lastLogin: '2026-03-28', createdAt: '2026-01-16' },
    { id: '3', name: 'Prof. João Santos', email: 'joao.santos@istec.pt', role: 'teacher', status: 'active', lastLogin: '2026-03-30', createdAt: '2026-01-10' },
    { id: '4', name: 'Prof. Maria Silva', email: 'maria.silva@istec.pt', role: 'teacher', status: 'active', lastLogin: '2026-03-29', createdAt: '2026-01-10' },
    { id: '5', name: 'Carla Mendes', email: 'carla.mendes@istec.pt', role: 'student', status: 'active', lastLogin: '2026-03-30', createdAt: '2026-01-20' },
    { id: '6', name: 'Daniel Rodrigues', email: 'daniel.rodrigues@istec.pt', role: 'student', status: 'inactive', lastLogin: '2026-03-15', createdAt: '2026-01-18' },
  ]);

  useEffect(() => {
    if (!user || user.role !== 'admin') {
      navigate('/login');
    }
  }, [user, navigate]);

  if (!user || user.role !== 'admin') {
    return null;
  }

  const totalUsers = systemUsers.length;
  const activeUsers = systemUsers.filter(u => u.status === 'active').length;
  const studentCount = systemUsers.filter(u => u.role === 'student').length;
  const teacherCount = systemUsers.filter(u => u.role === 'teacher').length;

  // Mock data for system activity
  const activityData = [
    { date: '24/03', users: 12, sessions: 45, scenarios: 8 },
    { date: '25/03', users: 15, sessions: 52, scenarios: 10 },
    { date: '26/03', users: 18, sessions: 61, scenarios: 12 },
    { date: '27/03', users: 14, sessions: 48, scenarios: 9 },
    { date: '28/03', users: 20, sessions: 68, scenarios: 15 },
    { date: '29/03', users: 22, sessions: 75, scenarios: 18 },
    { date: '30/03', users: 25, sessions: 82, scenarios: 20 },
  ];

  const systemHealth = [
    { component: 'API Gateway', status: 'operational', uptime: '99.9%' },
    { component: 'Database MySQL', status: 'operational', uptime: '99.8%' },
    { component: 'Simulation Service', status: 'operational', uptime: '99.7%' },
    { component: 'Authentication', status: 'operational', uptime: '100%' },
    { component: 'Backup Service', status: 'operational', uptime: '100%' },
  ];

  return (
    <div className="min-h-screen bg-gray-50">
      <Header />
      
      <div className="max-w-7xl mx-auto p-6">
        <div className="mb-6">
          <h1 className="text-3xl mb-2">Painel de Administração</h1>
          <p className="text-muted-foreground">
            Gestão e monitorização da infraestrutura do sistema
          </p>
        </div>

        {/* System Overview */}
        <div className="grid md:grid-cols-4 gap-4 mb-8">
          <Card>
            <CardHeader className="pb-2">
              <CardDescription>Utilizadores Totais</CardDescription>
              <CardTitle className="text-3xl flex items-center gap-2">
                <Users className="w-6 h-6 text-primary" />
                {totalUsers}
              </CardTitle>
            </CardHeader>
            <CardContent>
              <p className="text-sm text-muted-foreground">
                {activeUsers} ativos
              </p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader className="pb-2">
              <CardDescription>Alunos</CardDescription>
              <CardTitle className="text-3xl flex items-center gap-2">
                <GraduationCap className="w-6 h-6 text-secondary" />
                {studentCount}
              </CardTitle>
            </CardHeader>
          </Card>

          <Card>
            <CardHeader className="pb-2">
              <CardDescription>Professores</CardDescription>
              <CardTitle className="text-3xl flex items-center gap-2">
                <Users className="w-6 h-6 text-primary" />
                {teacherCount}
              </CardTitle>
            </CardHeader>
          </Card>

          <Card>
            <CardHeader className="pb-2">
              <CardDescription>Estado do Sistema</CardDescription>
              <CardTitle className="text-3xl flex items-center gap-2">
                <Activity className="w-6 h-6 text-green-500" />
                Operacional
              </CardTitle>
            </CardHeader>
          </Card>
        </div>

        {/* System Activity Chart */}
        <Card className="mb-8">
          <CardHeader>
            <CardTitle>Atividade do Sistema (Últimos 7 dias)</CardTitle>
            <CardDescription>
              Utilizadores ativos, sessões e cenários criados
            </CardDescription>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <LineChart data={activityData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="date" />
                <YAxis />
                <Tooltip />
                <Legend />
                <Line type="monotone" dataKey="users" name="Utilizadores" stroke="#0C73B7" strokeWidth={2} />
                <Line type="monotone" dataKey="sessions" name="Sessões" stroke="#2da7df" strokeWidth={2} />
                <Line type="monotone" dataKey="scenarios" name="Cenários" stroke="#1d1d1b" strokeWidth={2} />
              </LineChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        {/* System Health */}
        <Card className="mb-8">
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Shield className="w-5 h-5 text-primary" />
              Saúde da Infraestrutura
            </CardTitle>
            <CardDescription>
              Estado dos componentes do sistema
            </CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-3">
              {systemHealth.map((item, index) => (
                <div key={index} className="flex items-center justify-between p-3 bg-muted rounded-lg">
                  <div className="flex items-center gap-3">
                    {item.status === 'operational' ? (
                      <CheckCircle className="w-5 h-5 text-green-500" />
                    ) : (
                      <AlertCircle className="w-5 h-5 text-yellow-500" />
                    )}
                    <div>
                      <p className="font-medium">{item.component}</p>
                      <p className="text-sm text-muted-foreground">Uptime: {item.uptime}</p>
                    </div>
                  </div>
                  <Badge className={item.status === 'operational' ? 'bg-green-500' : 'bg-yellow-500'}>
                    {item.status === 'operational' ? 'Operacional' : 'Atenção'}
                  </Badge>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        {/* Users Management */}
        <Card>
          <CardHeader>
            <div className="flex items-center justify-between">
              <div>
                <CardTitle>Gestão de Utilizadores</CardTitle>
                <CardDescription>
                  Lista de todos os utilizadores do sistema
                </CardDescription>
              </div>
              <Button>
                <Users className="w-4 h-4 mr-2" />
                Adicionar Utilizador
              </Button>
            </div>
          </CardHeader>
          <CardContent>
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Nome</TableHead>
                  <TableHead>Email</TableHead>
                  <TableHead>Tipo</TableHead>
                  <TableHead>Estado</TableHead>
                  <TableHead>Último Login</TableHead>
                  <TableHead>Data de Criação</TableHead>
                  <TableHead></TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {systemUsers.map((user) => (
                  <TableRow key={user.id}>
                    <TableCell className="font-medium">{user.name}</TableCell>
                    <TableCell className="text-muted-foreground">{user.email}</TableCell>
                    <TableCell>
                      <Badge variant={
                        user.role === 'admin' ? 'default' :
                        user.role === 'teacher' ? 'secondary' :
                        'outline'
                      }>
                        {user.role === 'admin' ? 'Administrador' :
                         user.role === 'teacher' ? 'Professor' :
                         'Aluno'}
                      </Badge>
                    </TableCell>
                    <TableCell>
                      <Badge className={user.status === 'active' ? 'bg-green-500' : 'bg-gray-400'}>
                        {user.status === 'active' ? 'Ativo' : 'Inativo'}
                      </Badge>
                    </TableCell>
                    <TableCell className="text-sm">
                      {new Date(user.lastLogin).toLocaleDateString('pt-PT')}
                    </TableCell>
                    <TableCell className="text-sm text-muted-foreground">
                      {new Date(user.createdAt).toLocaleDateString('pt-PT')}
                    </TableCell>
                    <TableCell>
                      <Button variant="ghost" size="sm">
                        Editar
                      </Button>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </CardContent>
        </Card>

        {/* Quick Actions */}
        <div className="mt-8 flex flex-wrap gap-4">
          <Button variant="outline">
            <Database className="w-4 h-4 mr-2" />
            Executar Backup
          </Button>
          <Button variant="outline">
            <Activity className="w-4 h-4 mr-2" />
            Ver Logs do Sistema
          </Button>
          <Button variant="outline">
            <Shield className="w-4 h-4 mr-2" />
            Configurações de Segurança
          </Button>
        </div>
      </div>
    </div>
  );
}
