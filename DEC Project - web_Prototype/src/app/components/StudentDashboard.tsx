import { useEffect } from 'react';
import { useNavigate } from 'react-router';
import { useApp } from '../context/AppContext';
import { Header } from './Header';
import { Button } from './ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './ui/card';
import { Progress } from './ui/progress';
import { Badge } from './ui/badge';
import { LineChart, Line, BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer, PieChart, Pie, Cell } from 'recharts';
import { TrendingUp, TrendingDown, PiggyBank, Target, Award, Plus } from 'lucide-react';
import { calculateEffortRate, calculateMonthsToGoal } from '../utils/financialCalculations';

export function StudentDashboard() {
  const { user, currentFamily, families, setFamilies, challenges, userProgress } = useApp();
  const navigate = useNavigate();

  useEffect(() => {
    if (!user || user.role !== 'student') {
      navigate('/login');
      return;
    }

    // Initialize with mock data if no families exist
    if (families.length === 0) {
      const mockFamily = {
        id: '1',
        name: 'Família Silva',
        location: 'Porto',
        members: 4,
        monthlyIncome: 2500,
        fixedExpenses: 1200,
        variableExpenses: 600,
        savingsGoal: 5000,
        savingsDeadline: 12,
        currentSavings: 1200,
      };
      setFamilies([mockFamily]);
    }
  }, [user, navigate, families.length, setFamilies]);

  if (!user || user.role !== 'student') {
    return null;
  }

  const family = currentFamily || families[0];
  
  if (!family) {
    return (
      <div className="min-h-screen bg-gray-50">
        <Header />
        <div className="max-w-7xl mx-auto p-6">
          <Card className="text-center py-12">
            <CardContent>
              <Target className="w-16 h-16 mx-auto mb-4 text-muted-foreground" />
              <h2 className="text-2xl mb-4">Bem-vindo ao DEC!</h2>
              <p className="text-muted-foreground mb-6">
                Começa por criar o teu primeiro cenário familiar
              </p>
              <Button onClick={() => navigate('/family')}>
                <Plus className="w-4 h-4 mr-2" />
                Criar Cenário Familiar
              </Button>
            </CardContent>
          </Card>
        </div>
      </div>
    );
  }

  const netIncome = family.monthlyIncome - family.fixedExpenses - family.variableExpenses;
  const effortRate = calculateEffortRate(family.monthlyIncome, family.fixedExpenses);
  const monthsToGoal = calculateMonthsToGoal(family.currentSavings, family.savingsGoal, netIncome, 0.02);
  const savingsProgress = (family.currentSavings / family.savingsGoal) * 100;

  // Mock data for charts
  const monthlyData = [
    { month: 'Jan', rendimentos: 2500, despesas: 1800, poupanca: 700 },
    { month: 'Fev', rendimentos: 2500, despesas: 1750, poupanca: 750 },
    { month: 'Mar', rendimentos: 2500, despesas: 1850, poupanca: 650 },
    { month: 'Abr', rendimentos: 2500, despesas: 1700, poupanca: 800 },
    { month: 'Mai', rendimentos: 2500, despesas: 1800, poupanca: 700 },
    { month: 'Jun', rendimentos: 2500, despesas: 1780, poupanca: 720 },
  ];

  const expenseCategories = [
    { name: 'Habitação', value: family.fixedExpenses * 0.5, color: '#0C73B7' },
    { name: 'Alimentação', value: family.variableExpenses * 0.4, color: '#2da7df' },
    { name: 'Transportes', value: family.variableExpenses * 0.3, color: '#4db8e8' },
    { name: 'Lazer', value: family.variableExpenses * 0.2, color: '#085a8f' },
    { name: 'Outros', value: family.variableExpenses * 0.1, color: '#1d1d1b' },
  ];

  const activeChallenges = challenges.filter(c => !c.completed);
  const completedChallenges = challenges.filter(c => c.completed);

  return (
    <div className="min-h-screen bg-gray-50">
      <Header />
      
      <div className="max-w-7xl mx-auto p-6">
        {/* Header Section */}
        <div className="mb-8">
          <h1 className="text-3xl mb-2">Olá, {user.name}!</h1>
          <p className="text-muted-foreground">
            Aqui está o resumo financeiro do teu cenário: <strong>{family.name}</strong>
          </p>
        </div>

        {/* Key Metrics */}
        <div className="grid md:grid-cols-4 gap-4 mb-8">
          <Card>
            <CardHeader className="pb-2">
              <CardDescription>Saldo Mensal</CardDescription>
              <CardTitle className="text-2xl flex items-center gap-2">
                €{netIncome.toFixed(2)}
                {netIncome > 0 ? (
                  <TrendingUp className="w-5 h-5 text-green-500" />
                ) : (
                  <TrendingDown className="w-5 h-5 text-red-500" />
                )}
              </CardTitle>
            </CardHeader>
          </Card>

          <Card>
            <CardHeader className="pb-2">
              <CardDescription>Taxa de Esforço</CardDescription>
              <CardTitle className="text-2xl">{effortRate.toFixed(1)}%</CardTitle>
            </CardHeader>
            <CardContent>
              <Progress value={effortRate} className="h-2" />
              <p className="text-xs text-muted-foreground mt-2">
                {effortRate < 30 ? 'Ótimo!' : effortRate < 40 ? 'Aceitável' : 'Atenção!'}
              </p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader className="pb-2">
              <CardDescription>Poupança Atual</CardDescription>
              <CardTitle className="text-2xl flex items-center gap-2">
                <PiggyBank className="w-5 h-5 text-primary" />
                €{family.currentSavings}
              </CardTitle>
            </CardHeader>
          </Card>

          <Card>
            <CardHeader className="pb-2">
              <CardDescription>Objetivo de Poupança</CardDescription>
              <CardTitle className="text-2xl">€{family.savingsGoal}</CardTitle>
            </CardHeader>
            <CardContent>
              <Progress value={savingsProgress} className="h-2" />
              <p className="text-xs text-muted-foreground mt-2">
                {monthsToGoal} meses para atingir
              </p>
            </CardContent>
          </Card>
        </div>

        {/* Charts Row */}
        <div className="grid md:grid-cols-2 gap-6 mb-8">
          <Card>
            <CardHeader>
              <CardTitle>Evolução Mensal</CardTitle>
              <CardDescription>Rendimentos, despesas e poupança</CardDescription>
            </CardHeader>
            <CardContent>
              <ResponsiveContainer width="100%" height={300}>
                <LineChart data={monthlyData}>
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis dataKey="month" />
                  <YAxis />
                  <Tooltip />
                  <Legend />
                  <Line type="monotone" dataKey="rendimentos" stroke="#0C73B7" strokeWidth={2} />
                  <Line type="monotone" dataKey="despesas" stroke="#d4183d" strokeWidth={2} />
                  <Line type="monotone" dataKey="poupanca" stroke="#2da7df" strokeWidth={2} />
                </LineChart>
              </ResponsiveContainer>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle>Distribuição de Despesas</CardTitle>
              <CardDescription>Por categoria</CardDescription>
            </CardHeader>
            <CardContent className="flex items-center justify-center">
              <ResponsiveContainer width="100%" height={300}>
                <PieChart>
                  <Pie
                    data={expenseCategories}
                    cx="50%"
                    cy="50%"
                    labelLine={false}
                    label={({ name, percent }) => `${name} ${(percent * 100).toFixed(0)}%`}
                    outerRadius={100}
                    fill="#8884d8"
                    dataKey="value"
                  >
                    {expenseCategories.map((entry, index) => (
                      <Cell key={`cell-${index}`} fill={entry.color} />
                    ))}
                  </Pie>
                  <Tooltip />
                </PieChart>
              </ResponsiveContainer>
            </CardContent>
          </Card>
        </div>

        {/* Challenges and Achievements */}
        <div className="grid md:grid-cols-2 gap-6">
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Target className="w-5 h-5 text-primary" />
                Desafios Ativos
              </CardTitle>
            </CardHeader>
            <CardContent>
              {activeChallenges.length === 0 ? (
                <p className="text-muted-foreground text-center py-4">
                  Nenhum desafio ativo no momento
                </p>
              ) : (
                <div className="space-y-3">
                  {activeChallenges.slice(0, 3).map((challenge) => (
                    <div key={challenge.id} className="flex items-center justify-between p-3 bg-muted rounded-lg">
                      <div>
                        <p className="font-medium">{challenge.title}</p>
                        <p className="text-sm text-muted-foreground">{challenge.points} pontos</p>
                      </div>
                      <Button size="sm" onClick={() => navigate('/challenges')}>
                        Ver
                      </Button>
                    </div>
                  ))}
                </div>
              )}
              {activeChallenges.length > 0 && (
                <Button variant="outline" className="w-full mt-4" onClick={() => navigate('/challenges')}>
                  Ver Todos os Desafios
                </Button>
              )}
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Award className="w-5 h-5 text-primary" />
                Conquistas
              </CardTitle>
            </CardHeader>
            <CardContent>
              <div className="space-y-4">
                <div>
                  <p className="text-sm text-muted-foreground mb-2">Badges Conquistadas</p>
                  <div className="flex flex-wrap gap-2">
                    {userProgress.badges.length === 0 ? (
                      <p className="text-muted-foreground">Nenhuma badge ainda</p>
                    ) : (
                      userProgress.badges.map((badge, index) => (
                        <Badge key={index} variant="secondary" className="text-sm">
                          {badge}
                        </Badge>
                      ))
                    )}
                  </div>
                </div>
                <div>
                  <p className="text-sm text-muted-foreground mb-2">Progresso</p>
                  <div className="space-y-2">
                    <div className="flex justify-between text-sm">
                      <span>Desafios Concluídos</span>
                      <span>{completedChallenges.length}</span>
                    </div>
                    <div className="flex justify-between text-sm">
                      <span>Poupança Total</span>
                      <span>€{family.currentSavings}</span>
                    </div>
                  </div>
                </div>
              </div>
            </CardContent>
          </Card>
        </div>

        {/* Quick Actions */}
        <div className="mt-8 flex flex-wrap gap-4">
          <Button onClick={() => navigate('/family')}>
            Gerir Cenário Familiar
          </Button>
          <Button variant="outline" onClick={() => navigate('/transactions')}>
            Registar Lançamento
          </Button>
          <Button variant="outline" onClick={() => navigate('/simulations')}>
            Executar Simulação
          </Button>
        </div>
      </div>
    </div>
  );
}
