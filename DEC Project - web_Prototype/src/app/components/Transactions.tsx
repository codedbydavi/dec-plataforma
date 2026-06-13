import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router';
import { useApp } from '../context/AppContext';
import { Header } from './Header';
import { Button } from './ui/button';
import { Input } from './ui/input';
import { Label } from './ui/label';
import { Textarea } from './ui/textarea';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './ui/card';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from './ui/select';
import { Switch } from './ui/switch';
import { Badge } from './ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from './ui/tabs';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from './ui/table';
import { toast } from 'sonner';
import { TrendingUp, TrendingDown, Trash2, Plus } from 'lucide-react';

const INCOME_CATEGORIES = [
  'Salário',
  'Subsídio',
  'Prestação de Serviços',
  'Investimentos',
  'Pensão',
  'Apoios',
  'Outros Rendimentos',
];

const EXPENSE_CATEGORIES = [
  'Habitação',
  'Alimentação',
  'Transportes',
  'Saúde',
  'Educação',
  'Lazer',
  'Vestuário',
  'Telecomunicações',
  'Seguros',
  'Outros',
];

export function Transactions() {
  const { user, currentFamily, transactions, setTransactions } = useApp();
  const navigate = useNavigate();

  const [type, setType] = useState<'income' | 'expense'>('expense');
  const [category, setCategory] = useState('');
  const [amount, setAmount] = useState(0);
  const [description, setDescription] = useState('');
  const [date, setDate] = useState(new Date().toISOString().split('T')[0]);
  const [isRecurring, setIsRecurring] = useState(false);

  useEffect(() => {
    if (!user) {
      navigate('/login');
      return;
    }

    if (!currentFamily) {
      toast.error('Cria primeiro um cenário familiar');
      navigate('/family');
    }
  }, [user, currentFamily, navigate]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (!currentFamily) return;

    const newTransaction = {
      id: Date.now().toString(),
      scenarioId: currentFamily.id,
      type,
      category,
      amount,
      description,
      date,
      isRecurring,
    };

    setTransactions([...transactions, newTransaction]);
    toast.success(`${type === 'income' ? 'Rendimento' : 'Despesa'} registado com sucesso!`);

    // Reset form
    setAmount(0);
    setDescription('');
    setCategory('');
  };

  const handleDelete = (id: string) => {
    setTransactions(transactions.filter(t => t.id !== id));
    toast.success('Lançamento eliminado');
  };

  if (!user || !currentFamily) {
    return null;
  }

  const familyTransactions = transactions.filter(t => t.scenarioId === currentFamily.id);
  const totalIncome = familyTransactions
    .filter(t => t.type === 'income')
    .reduce((sum, t) => sum + t.amount, 0);
  const totalExpenses = familyTransactions
    .filter(t => t.type === 'expense')
    .reduce((sum, t) => sum + t.amount, 0);
  const balance = totalIncome - totalExpenses;

  return (
    <div className="min-h-screen bg-gray-50">
      <Header />
      
      <div className="max-w-6xl mx-auto p-6">
        <div className="mb-6">
          <h1 className="text-3xl mb-2">Lançamentos Financeiros</h1>
          <p className="text-muted-foreground">
            Regista rendimentos e despesas do cenário: <strong>{currentFamily.name}</strong>
          </p>
        </div>

        {/* Summary Cards */}
        <div className="grid md:grid-cols-3 gap-4 mb-6">
          <Card>
            <CardHeader className="pb-2">
              <CardDescription>Total de Rendimentos</CardDescription>
              <CardTitle className="text-2xl flex items-center gap-2 text-green-600">
                <TrendingUp className="w-5 h-5" />
                €{totalIncome.toFixed(2)}
              </CardTitle>
            </CardHeader>
          </Card>

          <Card>
            <CardHeader className="pb-2">
              <CardDescription>Total de Despesas</CardDescription>
              <CardTitle className="text-2xl flex items-center gap-2 text-red-600">
                <TrendingDown className="w-5 h-5" />
                €{totalExpenses.toFixed(2)}
              </CardTitle>
            </CardHeader>
          </Card>

          <Card>
            <CardHeader className="pb-2">
              <CardDescription>Saldo</CardDescription>
              <CardTitle className={`text-2xl ${balance >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                €{balance.toFixed(2)}
              </CardTitle>
            </CardHeader>
          </Card>
        </div>

        <Tabs defaultValue="register" className="space-y-6">
          <TabsList>
            <TabsTrigger value="register">Registar Lançamento</TabsTrigger>
            <TabsTrigger value="history">Histórico</TabsTrigger>
          </TabsList>

          <TabsContent value="register">
            <Card>
              <CardHeader>
                <CardTitle>Novo Lançamento</CardTitle>
                <CardDescription>
                  Adiciona um novo rendimento ou despesa
                </CardDescription>
              </CardHeader>
              <CardContent>
                <form onSubmit={handleSubmit} className="space-y-6">
                  <div className="grid md:grid-cols-2 gap-6">
                    <div className="space-y-2">
                      <Label htmlFor="type">Tipo de Lançamento</Label>
                      <Select value={type} onValueChange={(value: 'income' | 'expense') => {
                        setType(value);
                        setCategory('');
                      }}>
                        <SelectTrigger id="type">
                          <SelectValue />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectItem value="income">
                            <div className="flex items-center gap-2">
                              <TrendingUp className="w-4 h-4 text-green-600" />
                              Rendimento
                            </div>
                          </SelectItem>
                          <SelectItem value="expense">
                            <div className="flex items-center gap-2">
                              <TrendingDown className="w-4 h-4 text-red-600" />
                              Despesa
                            </div>
                          </SelectItem>
                        </SelectContent>
                      </Select>
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="category">Categoria</Label>
                      <Select value={category} onValueChange={setCategory}>
                        <SelectTrigger id="category">
                          <SelectValue placeholder="Seleciona a categoria" />
                        </SelectTrigger>
                        <SelectContent>
                          {(type === 'income' ? INCOME_CATEGORIES : EXPENSE_CATEGORIES).map(cat => (
                            <SelectItem key={cat} value={cat}>{cat}</SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="amount">Valor (€)</Label>
                      <Input
                        id="amount"
                        type="number"
                        min="0"
                        step="0.01"
                        value={amount}
                        onChange={(e) => setAmount(parseFloat(e.target.value))}
                        required
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="date">Data</Label>
                      <Input
                        id="date"
                        type="date"
                        value={date}
                        onChange={(e) => setDate(e.target.value)}
                        required
                      />
                    </div>
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="description">Descrição</Label>
                    <Textarea
                      id="description"
                      placeholder="Adiciona uma descrição opcional..."
                      value={description}
                      onChange={(e) => setDescription(e.target.value)}
                      rows={3}
                    />
                  </div>

                  <div className="flex items-center space-x-2">
                    <Switch
                      id="recurring"
                      checked={isRecurring}
                      onCheckedChange={setIsRecurring}
                    />
                    <Label htmlFor="recurring">Lançamento recorrente (mensal)</Label>
                  </div>

                  <Button type="submit" size="lg">
                    <Plus className="w-4 h-4 mr-2" />
                    Registar Lançamento
                  </Button>
                </form>
              </CardContent>
            </Card>
          </TabsContent>

          <TabsContent value="history">
            <Card>
              <CardHeader>
                <CardTitle>Histórico de Lançamentos</CardTitle>
                <CardDescription>
                  Todos os lançamentos registados para este cenário
                </CardDescription>
              </CardHeader>
              <CardContent>
                {familyTransactions.length === 0 ? (
                  <div className="text-center py-8">
                    <p className="text-muted-foreground">Nenhum lançamento registado</p>
                  </div>
                ) : (
                  <Table>
                    <TableHeader>
                      <TableRow>
                        <TableHead>Data</TableHead>
                        <TableHead>Tipo</TableHead>
                        <TableHead>Categoria</TableHead>
                        <TableHead>Descrição</TableHead>
                        <TableHead className="text-right">Valor</TableHead>
                        <TableHead></TableHead>
                      </TableRow>
                    </TableHeader>
                    <TableBody>
                      {familyTransactions
                        .sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime())
                        .map((transaction) => (
                          <TableRow key={transaction.id}>
                            <TableCell>
                              {new Date(transaction.date).toLocaleDateString('pt-PT')}
                            </TableCell>
                            <TableCell>
                              {transaction.type === 'income' ? (
                                <Badge className="bg-green-500">
                                  <TrendingUp className="w-3 h-3 mr-1" />
                                  Rendimento
                                </Badge>
                              ) : (
                                <Badge variant="destructive">
                                  <TrendingDown className="w-3 h-3 mr-1" />
                                  Despesa
                                </Badge>
                              )}
                            </TableCell>
                            <TableCell>{transaction.category}</TableCell>
                            <TableCell className="max-w-xs truncate">
                              {transaction.description || '—'}
                              {transaction.isRecurring && (
                                <Badge variant="outline" className="ml-2 text-xs">
                                  Recorrente
                                </Badge>
                              )}
                            </TableCell>
                            <TableCell className={`text-right font-medium ${
                              transaction.type === 'income' ? 'text-green-600' : 'text-red-600'
                            }`}>
                              {transaction.type === 'income' ? '+' : '-'}€{transaction.amount.toFixed(2)}
                            </TableCell>
                            <TableCell>
                              <Button
                                variant="ghost"
                                size="sm"
                                onClick={() => handleDelete(transaction.id)}
                              >
                                <Trash2 className="w-4 h-4 text-destructive" />
                              </Button>
                            </TableCell>
                          </TableRow>
                        ))}
                    </TableBody>
                  </Table>
                )}
              </CardContent>
            </Card>
          </TabsContent>
        </Tabs>
      </div>
    </div>
  );
}
