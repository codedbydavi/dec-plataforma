import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router';
import { useApp } from '../context/AppContext';
import { Header } from './Header';
import { Button } from './ui/button';
import { Input } from './ui/input';
import { Label } from './ui/label';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './ui/card';
import { Tabs, TabsContent, TabsList, TabsTrigger } from './ui/tabs';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from './ui/select';
import { toast } from 'sonner';
import { Users, MapPin, Euro, Home, ShoppingCart, Sparkles } from 'lucide-react';

export function FamilyScenario() {
  const { user, families, setFamilies, currentFamily, setCurrentFamily } = useApp();
  const navigate = useNavigate();

  const [name, setName] = useState('');
  const [location, setLocation] = useState('');
  const [members, setMembers] = useState(1);
  const [monthlyIncome, setMonthlyIncome] = useState(0);
  const [fixedExpenses, setFixedExpenses] = useState(0);
  const [variableExpenses, setVariableExpenses] = useState(0);
  const [savingsGoal, setSavingsGoal] = useState(0);
  const [savingsDeadline, setSavingsDeadline] = useState(12);
  const [currentSavings, setCurrentSavings] = useState(0);
  const [editMode, setEditMode] = useState(false);

  useEffect(() => {
    if (!user) {
      navigate('/login');
      return;
    }

    if (currentFamily) {
      setName(currentFamily.name);
      setLocation(currentFamily.location);
      setMembers(currentFamily.members);
      setMonthlyIncome(currentFamily.monthlyIncome);
      setFixedExpenses(currentFamily.fixedExpenses);
      setVariableExpenses(currentFamily.variableExpenses);
      setSavingsGoal(currentFamily.savingsGoal);
      setSavingsDeadline(currentFamily.savingsDeadline);
      setCurrentSavings(currentFamily.currentSavings);
      setEditMode(true);
    }
  }, [user, currentFamily, navigate]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    const familyData = {
      id: editMode && currentFamily ? currentFamily.id : Date.now().toString(),
      name,
      location,
      members,
      monthlyIncome,
      fixedExpenses,
      variableExpenses,
      savingsGoal,
      savingsDeadline,
      currentSavings,
    };

    if (editMode && currentFamily) {
      const updatedFamilies = families.map(f => 
        f.id === currentFamily.id ? familyData : f
      );
      setFamilies(updatedFamilies);
      setCurrentFamily(familyData);
      toast.success('Cenário atualizado com sucesso!');
    } else {
      setFamilies([...families, familyData]);
      setCurrentFamily(familyData);
      toast.success('Cenário criado com sucesso!');
    }

    navigate('/dashboard');
  };

  const handlePresetScenario = (preset: string) => {
    switch (preset) {
      case 'student':
        setName('Estudante Deslocado');
        setLocation('Porto');
        setMembers(1);
        setMonthlyIncome(800);
        setFixedExpenses(350);
        setVariableExpenses(250);
        setSavingsGoal(1000);
        setSavingsDeadline(12);
        setCurrentSavings(200);
        break;
      case 'single':
        setName('Família Monoparental');
        setLocation('Lisboa');
        setMembers(2);
        setMonthlyIncome(1500);
        setFixedExpenses(700);
        setVariableExpenses(450);
        setSavingsGoal(3000);
        setSavingsDeadline(18);
        setCurrentSavings(500);
        break;
      case 'family':
        setName('Família com 2 Filhos');
        setLocation('Porto');
        setMembers(4);
        setMonthlyIncome(2500);
        setFixedExpenses(1200);
        setVariableExpenses(600);
        setSavingsGoal(5000);
        setSavingsDeadline(24);
        setCurrentSavings(1000);
        break;
      case 'young':
        setName('Casal Jovem');
        setLocation('Braga');
        setMembers(2);
        setMonthlyIncome(2000);
        setFixedExpenses(800);
        setVariableExpenses(500);
        setSavingsGoal(10000);
        setSavingsDeadline(36);
        setCurrentSavings(2000);
        break;
    }
    toast.success('Cenário pré-definido carregado!');
  };

  if (!user) {
    return null;
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Header />
      
      <div className="max-w-4xl mx-auto p-6">
        <div className="mb-6">
          <h1 className="text-3xl mb-2">
            {editMode ? 'Editar Cenário Familiar' : 'Criar Cenário Familiar'}
          </h1>
          <p className="text-muted-foreground">
            Define o agregado familiar e os parâmetros financeiros para simulações
          </p>
        </div>

        <Tabs defaultValue="manual" className="mb-6">
          <TabsList className="grid w-full grid-cols-2">
            <TabsTrigger value="manual">Criar Manualmente</TabsTrigger>
            <TabsTrigger value="preset">Cenários Pré-definidos</TabsTrigger>
          </TabsList>

          <TabsContent value="preset">
            <Card>
              <CardHeader>
                <CardTitle>Escolhe um Cenário</CardTitle>
                <CardDescription>
                  Seleciona um cenário pré-definido para começar rapidamente
                </CardDescription>
              </CardHeader>
              <CardContent className="grid md:grid-cols-2 gap-4">
                <Button
                  variant="outline"
                  className="h-auto flex-col items-start p-4 text-left"
                  onClick={() => handlePresetScenario('student')}
                >
                  <div className="flex items-center gap-2 mb-2">
                    <Users className="w-5 h-5 text-primary" />
                    <span className="font-semibold">Estudante Deslocado</span>
                  </div>
                  <p className="text-sm text-muted-foreground">
                    1 pessoa, €800/mês, vida simples
                  </p>
                </Button>

                <Button
                  variant="outline"
                  className="h-auto flex-col items-start p-4 text-left"
                  onClick={() => handlePresetScenario('single')}
                >
                  <div className="flex items-center gap-2 mb-2">
                    <Home className="w-5 h-5 text-primary" />
                    <span className="font-semibold">Família Monoparental</span>
                  </div>
                  <p className="text-sm text-muted-foreground">
                    2 pessoas, €1500/mês, desafios médios
                  </p>
                </Button>

                <Button
                  variant="outline"
                  className="h-auto flex-col items-start p-4 text-left"
                  onClick={() => handlePresetScenario('family')}
                >
                  <div className="flex items-center gap-2 mb-2">
                    <Users className="w-5 h-5 text-primary" />
                    <span className="font-semibold">Família com 2 Filhos</span>
                  </div>
                  <p className="text-sm text-muted-foreground">
                    4 pessoas, €2500/mês, gestão complexa
                  </p>
                </Button>

                <Button
                  variant="outline"
                  className="h-auto flex-col items-start p-4 text-left"
                  onClick={() => handlePresetScenario('young')}
                >
                  <div className="flex items-center gap-2 mb-2">
                    <Sparkles className="w-5 h-5 text-primary" />
                    <span className="font-semibold">Casal Jovem</span>
                  </div>
                  <p className="text-sm text-muted-foreground">
                    2 pessoas, €2000/mês, poupança ambiciosa
                  </p>
                </Button>
              </CardContent>
            </Card>
          </TabsContent>

          <TabsContent value="manual">
            <form onSubmit={handleSubmit} className="space-y-6">
              {/* Basic Info */}
              <Card>
                <CardHeader>
                  <CardTitle>Informação Básica</CardTitle>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="space-y-2">
                    <Label htmlFor="name">Nome do Cenário</Label>
                    <Input
                      id="name"
                      placeholder="ex: Família Silva"
                      value={name}
                      onChange={(e) => setName(e.target.value)}
                      required
                    />
                  </div>

                  <div className="grid md:grid-cols-2 gap-4">
                    <div className="space-y-2">
                      <Label htmlFor="location">
                        <MapPin className="w-4 h-4 inline mr-1" />
                        Localização
                      </Label>
                      <Select value={location} onValueChange={setLocation}>
                        <SelectTrigger id="location">
                          <SelectValue placeholder="Seleciona a cidade" />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectItem value="Porto">Porto</SelectItem>
                          <SelectItem value="Lisboa">Lisboa</SelectItem>
                          <SelectItem value="Braga">Braga</SelectItem>
                          <SelectItem value="Coimbra">Coimbra</SelectItem>
                          <SelectItem value="Faro">Faro</SelectItem>
                          <SelectItem value="Aveiro">Aveiro</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="members">
                        <Users className="w-4 h-4 inline mr-1" />
                        Membros do Agregado
                      </Label>
                      <Input
                        id="members"
                        type="number"
                        min="1"
                        value={members}
                        onChange={(e) => setMembers(parseInt(e.target.value))}
                        required
                      />
                    </div>
                  </div>
                </CardContent>
              </Card>

              {/* Financial Info */}
              <Card>
                <CardHeader>
                  <CardTitle>Rendimentos e Despesas</CardTitle>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="space-y-2">
                    <Label htmlFor="income">
                      <Euro className="w-4 h-4 inline mr-1" />
                      Rendimento Mensal Total (€)
                    </Label>
                    <Input
                      id="income"
                      type="number"
                      min="0"
                      step="0.01"
                      value={monthlyIncome}
                      onChange={(e) => setMonthlyIncome(parseFloat(e.target.value))}
                      required
                    />
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="fixed">
                      <Home className="w-4 h-4 inline mr-1" />
                      Despesas Fixas Mensais (€)
                    </Label>
                    <Input
                      id="fixed"
                      type="number"
                      min="0"
                      step="0.01"
                      value={fixedExpenses}
                      onChange={(e) => setFixedExpenses(parseFloat(e.target.value))}
                      required
                    />
                    <p className="text-xs text-muted-foreground">
                      Renda, água, luz, telecomunicações, seguros, etc.
                    </p>
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="variable">
                      <ShoppingCart className="w-4 h-4 inline mr-1" />
                      Despesas Variáveis Mensais (€)
                    </Label>
                    <Input
                      id="variable"
                      type="number"
                      min="0"
                      step="0.01"
                      value={variableExpenses}
                      onChange={(e) => setVariableExpenses(parseFloat(e.target.value))}
                      required
                    />
                    <p className="text-xs text-muted-foreground">
                      Alimentação, transportes, lazer, vestuário, etc.
                    </p>
                  </div>

                  <div className="bg-muted p-4 rounded-lg">
                    <div className="flex justify-between mb-2">
                      <span>Total de Despesas:</span>
                      <span className="font-semibold">€{(fixedExpenses + variableExpenses).toFixed(2)}</span>
                    </div>
                    <div className="flex justify-between">
                      <span>Saldo Disponível:</span>
                      <span className={`font-semibold ${monthlyIncome - fixedExpenses - variableExpenses >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                        €{(monthlyIncome - fixedExpenses - variableExpenses).toFixed(2)}
                      </span>
                    </div>
                  </div>
                </CardContent>
              </Card>

              {/* Savings Goals */}
              <Card>
                <CardHeader>
                  <CardTitle>Objetivo de Poupança</CardTitle>
                </CardHeader>
                <CardContent className="space-y-4">
                  <div className="space-y-2">
                    <Label htmlFor="current-savings">Poupança Atual (€)</Label>
                    <Input
                      id="current-savings"
                      type="number"
                      min="0"
                      step="0.01"
                      value={currentSavings}
                      onChange={(e) => setCurrentSavings(parseFloat(e.target.value))}
                      required
                    />
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="goal">Meta de Poupança (€)</Label>
                    <Input
                      id="goal"
                      type="number"
                      min="0"
                      step="0.01"
                      value={savingsGoal}
                      onChange={(e) => setSavingsGoal(parseFloat(e.target.value))}
                      required
                    />
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="deadline">Prazo para Atingir (meses)</Label>
                    <Input
                      id="deadline"
                      type="number"
                      min="1"
                      value={savingsDeadline}
                      onChange={(e) => setSavingsDeadline(parseInt(e.target.value))}
                      required
                    />
                  </div>
                </CardContent>
              </Card>

              <div className="flex gap-4">
                <Button type="submit" size="lg">
                  {editMode ? 'Atualizar Cenário' : 'Criar Cenário'}
                </Button>
                <Button type="button" variant="outline" size="lg" onClick={() => navigate('/dashboard')}>
                  Cancelar
                </Button>
              </div>
            </form>
          </TabsContent>
        </Tabs>
      </div>
    </div>
  );
}
