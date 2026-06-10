import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router';
import { useApp } from '../context/AppContext';
import { Header } from './Header';
import { Button } from './ui/button';
import { Input } from './ui/input';
import { Label } from './ui/label';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './ui/card';
import { Tabs, TabsContent, TabsList, TabsTrigger } from './ui/tabs';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from './ui/table';
import { LineChart, Line, BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import { toast } from 'sonner';
import {
  calculateSavingsProjection,
  calculateLoanAmortization,
  calculateCashFlowProjection,
  calculateTotalCreditCost,
} from '../utils/financialCalculations';
import { TrendingUp, Calculator, PiggyBank, CreditCard, ArrowRight } from 'lucide-react';

export function Simulations() {
  const { user, currentFamily } = useApp();
  const navigate = useNavigate();

  // Savings simulation state
  const [savingsPrincipal, setSavingsPrincipal] = useState(1000);
  const [savingsMonthly, setSavingsMonthly] = useState(200);
  const [savingsRate, setSavingsRate] = useState(2);
  const [savingsMonths, setSavingsMonths] = useState(12);
  const [savingsResult, setSavingsResult] = useState<any[]>([]);

  // Loan simulation state
  const [loanPrincipal, setLoanPrincipal] = useState(50000);
  const [loanRate, setLoanRate] = useState(5);
  const [loanMonths, setLoanMonths] = useState(240);
  const [loanResult, setLoanResult] = useState<any[]>([]);
  const [loanTotalCost, setLoanTotalCost] = useState<any>(null);

  // Cash flow simulation state
  const [cashFlowIncome, setCashFlowIncome] = useState(2500);
  const [cashFlowFixed, setCashFlowFixed] = useState(1200);
  const [cashFlowVariable, setCashFlowVariable] = useState(600);
  const [cashFlowInflation, setCashFlowInflation] = useState(3);
  const [cashFlowMonths, setCashFlowMonths] = useState(24);
  const [cashFlowResult, setCashFlowResult] = useState<any[]>([]);

  useEffect(() => {
    if (!user) {
      navigate('/login');
      return;
    }

    if (!currentFamily) {
      toast.error('Cria primeiro um cenário familiar');
      navigate('/family');
      return;
    }

    // Initialize with family data
    setSavingsPrincipal(currentFamily.currentSavings);
    setSavingsMonthly(Math.max(0, currentFamily.monthlyIncome - currentFamily.fixedExpenses - currentFamily.variableExpenses));
    setCashFlowIncome(currentFamily.monthlyIncome);
    setCashFlowFixed(currentFamily.fixedExpenses);
    setCashFlowVariable(currentFamily.variableExpenses);
  }, [user, currentFamily, navigate]);

  const handleSavingsSimulation = () => {
    const projection = calculateSavingsProjection(
      savingsPrincipal,
      savingsMonthly,
      savingsRate / 100,
      savingsMonths
    );
    setSavingsResult(projection);
    toast.success('Simulação de poupança calculada!');
  };

  const handleLoanSimulation = () => {
    const amortization = calculateLoanAmortization(
      loanPrincipal,
      loanRate / 100,
      loanMonths
    );
    setLoanResult(amortization.slice(0, 12)); // Show first 12 months in table
    
    const totalCost = calculateTotalCreditCost(
      loanPrincipal,
      loanRate / 100,
      loanMonths
    );
    setLoanTotalCost(totalCost);
    toast.success('Simulação de crédito calculada!');
  };

  const handleCashFlowSimulation = () => {
    const projection = calculateCashFlowProjection(
      cashFlowIncome,
      cashFlowFixed,
      cashFlowVariable,
      cashFlowInflation / 100,
      cashFlowMonths
    );
    setCashFlowResult(projection);
    toast.success('Projeção de fluxo de caixa calculada!');
  };

  if (!user || !currentFamily) {
    return null;
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Header />
      
      <div className="max-w-7xl mx-auto p-6">
        <div className="mb-6">
          <h1 className="text-3xl mb-2">Simulações Financeiras</h1>
          <p className="text-muted-foreground">
            Executa simulações de poupança, crédito e projeções de fluxo de caixa
          </p>
        </div>

        <Tabs defaultValue="savings" className="space-y-6">
          <TabsList className="grid w-full grid-cols-3">
            <TabsTrigger value="savings">
              <PiggyBank className="w-4 h-4 mr-2" />
              Poupança
            </TabsTrigger>
            <TabsTrigger value="loan">
              <CreditCard className="w-4 h-4 mr-2" />
              Crédito
            </TabsTrigger>
            <TabsTrigger value="cashflow">
              <TrendingUp className="w-4 h-4 mr-2" />
              Fluxo de Caixa
            </TabsTrigger>
          </TabsList>

          {/* Savings Simulation */}
          <TabsContent value="savings" className="space-y-6">
            <Card>
              <CardHeader>
                <CardTitle>Simulação de Poupança com Juros Compostos</CardTitle>
                <CardDescription>
                  Projeta o crescimento da tua poupança ao longo do tempo
                </CardDescription>
              </CardHeader>
              <CardContent className="space-y-6">
                <div className="grid md:grid-cols-2 gap-6">
                  <div className="space-y-4">
                    <div className="space-y-2">
                      <Label htmlFor="savings-principal">Capital Inicial (€)</Label>
                      <Input
                        id="savings-principal"
                        type="number"
                        min="0"
                        step="0.01"
                        value={savingsPrincipal}
                        onChange={(e) => setSavingsPrincipal(parseFloat(e.target.value))}
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="savings-monthly">Contribuição Mensal (€)</Label>
                      <Input
                        id="savings-monthly"
                        type="number"
                        min="0"
                        step="0.01"
                        value={savingsMonthly}
                        onChange={(e) => setSavingsMonthly(parseFloat(e.target.value))}
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="savings-rate">Taxa de Juro Anual (%)</Label>
                      <Input
                        id="savings-rate"
                        type="number"
                        min="0"
                        max="20"
                        step="0.1"
                        value={savingsRate}
                        onChange={(e) => setSavingsRate(parseFloat(e.target.value))}
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="savings-months">Período (meses)</Label>
                      <Input
                        id="savings-months"
                        type="number"
                        min="1"
                        max="360"
                        value={savingsMonths}
                        onChange={(e) => setSavingsMonths(parseInt(e.target.value))}
                      />
                    </div>

                    <Button onClick={handleSavingsSimulation} className="w-full">
                      <Calculator className="w-4 h-4 mr-2" />
                      Calcular Projeção
                    </Button>
                  </div>

                  {savingsResult.length > 0 && (
                    <div className="space-y-4">
                      <div className="bg-primary text-white p-6 rounded-lg">
                        <p className="text-sm opacity-90 mb-2">Valor Final Acumulado</p>
                        <p className="text-4xl">
                          €{savingsResult[savingsResult.length - 1].total.toFixed(2)}
                        </p>
                      </div>

                      <div className="grid grid-cols-2 gap-4">
                        <div className="bg-muted p-4 rounded-lg">
                          <p className="text-sm text-muted-foreground mb-1">Total Investido</p>
                          <p className="text-xl">
                            €{(savingsPrincipal + savingsMonthly * savingsMonths).toFixed(2)}
                          </p>
                        </div>
                        <div className="bg-muted p-4 rounded-lg">
                          <p className="text-sm text-muted-foreground mb-1">Juros Ganhos</p>
                          <p className="text-xl text-green-600">
                            €{(savingsResult[savingsResult.length - 1].total - savingsPrincipal - savingsMonthly * savingsMonths).toFixed(2)}
                          </p>
                        </div>
                      </div>
                    </div>
                  )}
                </div>

                {savingsResult.length > 0 && (
                  <div>
                    <h3 className="text-lg mb-4">Crescimento da Poupança</h3>
                    <ResponsiveContainer width="100%" height={300}>
                      <LineChart data={savingsResult}>
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis dataKey="month" label={{ value: 'Mês', position: 'insideBottom', offset: -5 }} />
                        <YAxis label={{ value: 'Valor (€)', angle: -90, position: 'insideLeft' }} />
                        <Tooltip />
                        <Legend />
                        <Line type="monotone" dataKey="total" name="Total Acumulado" stroke="#0C73B7" strokeWidth={2} />
                        <Line type="monotone" dataKey="balance" name="Capital" stroke="#2da7df" strokeWidth={2} />
                      </LineChart>
                    </ResponsiveContainer>
                  </div>
                )}
              </CardContent>
            </Card>
          </TabsContent>

          {/* Loan Simulation */}
          <TabsContent value="loan" className="space-y-6">
            <Card>
              <CardHeader>
                <CardTitle>Simulação de Crédito (Sistema Francês)</CardTitle>
                <CardDescription>
                  Calcula as prestações e custo total de um empréstimo
                </CardDescription>
              </CardHeader>
              <CardContent className="space-y-6">
                <div className="grid md:grid-cols-2 gap-6">
                  <div className="space-y-4">
                    <div className="space-y-2">
                      <Label htmlFor="loan-principal">Montante do Crédito (€)</Label>
                      <Input
                        id="loan-principal"
                        type="number"
                        min="0"
                        step="1000"
                        value={loanPrincipal}
                        onChange={(e) => setLoanPrincipal(parseFloat(e.target.value))}
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="loan-rate">Taxa de Juro Anual (TAN %)</Label>
                      <Input
                        id="loan-rate"
                        type="number"
                        min="0"
                        max="20"
                        step="0.1"
                        value={loanRate}
                        onChange={(e) => setLoanRate(parseFloat(e.target.value))}
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="loan-months">Prazo (meses)</Label>
                      <Input
                        id="loan-months"
                        type="number"
                        min="1"
                        max="480"
                        value={loanMonths}
                        onChange={(e) => setLoanMonths(parseInt(e.target.value))}
                      />
                      <p className="text-xs text-muted-foreground">
                        {(loanMonths / 12).toFixed(1)} anos
                      </p>
                    </div>

                    <Button onClick={handleLoanSimulation} className="w-full">
                      <Calculator className="w-4 h-4 mr-2" />
                      Calcular Plano de Amortização
                    </Button>
                  </div>

                  {loanTotalCost && (
                    <div className="space-y-4">
                      <div className="bg-destructive text-white p-6 rounded-lg">
                        <p className="text-sm opacity-90 mb-2">Custo Total do Crédito</p>
                        <p className="text-4xl">
                          €{loanTotalCost.totalCost.toFixed(2)}
                        </p>
                      </div>

                      <div className="grid grid-cols-2 gap-4">
                        <div className="bg-muted p-4 rounded-lg">
                          <p className="text-sm text-muted-foreground mb-1">Prestação Mensal</p>
                          <p className="text-xl">
                            €{loanResult.length > 0 ? loanResult[0].payment.toFixed(2) : '0.00'}
                          </p>
                        </div>
                        <div className="bg-muted p-4 rounded-lg">
                          <p className="text-sm text-muted-foreground mb-1">Total de Juros</p>
                          <p className="text-xl text-red-600">
                            €{loanTotalCost.totalInterest.toFixed(2)}
                          </p>
                        </div>
                      </div>

                      <div className="bg-yellow-50 border border-yellow-200 p-4 rounded-lg">
                        <p className="text-sm">
                          <strong>Taxa de Esforço:</strong> Se o teu rendimento mensal é €{currentFamily.monthlyIncome}, 
                          esta prestação representa <strong>{((loanResult[0]?.payment / currentFamily.monthlyIncome) * 100).toFixed(1)}%</strong> do teu rendimento.
                        </p>
                      </div>
                    </div>
                  )}
                </div>

                {loanResult.length > 0 && (
                  <div>
                    <h3 className="text-lg mb-4">Plano de Amortização (primeiros 12 meses)</h3>
                    <div className="overflow-x-auto">
                      <Table>
                        <TableHeader>
                          <TableRow>
                            <TableHead>Mês</TableHead>
                            <TableHead className="text-right">Prestação</TableHead>
                            <TableHead className="text-right">Capital</TableHead>
                            <TableHead className="text-right">Juros</TableHead>
                            <TableHead className="text-right">Saldo Devedor</TableHead>
                          </TableRow>
                        </TableHeader>
                        <TableBody>
                          {loanResult.map((row) => (
                            <TableRow key={row.month}>
                              <TableCell>{row.month}</TableCell>
                              <TableCell className="text-right">€{row.payment.toFixed(2)}</TableCell>
                              <TableCell className="text-right">€{row.principal.toFixed(2)}</TableCell>
                              <TableCell className="text-right text-red-600">€{row.interest.toFixed(2)}</TableCell>
                              <TableCell className="text-right">€{row.balance.toFixed(2)}</TableCell>
                            </TableRow>
                          ))}
                        </TableBody>
                      </Table>
                    </div>
                  </div>
                )}
              </CardContent>
            </Card>
          </TabsContent>

          {/* Cash Flow Simulation */}
          <TabsContent value="cashflow" className="space-y-6">
            <Card>
              <CardHeader>
                <CardTitle>Projeção de Fluxo de Caixa com Inflação</CardTitle>
                <CardDescription>
                  Projeta o saldo mensal considerando inflação nas despesas variáveis
                </CardDescription>
              </CardHeader>
              <CardContent className="space-y-6">
                <div className="grid md:grid-cols-2 gap-6">
                  <div className="space-y-4">
                    <div className="space-y-2">
                      <Label htmlFor="cf-income">Rendimento Mensal (€)</Label>
                      <Input
                        id="cf-income"
                        type="number"
                        min="0"
                        step="0.01"
                        value={cashFlowIncome}
                        onChange={(e) => setCashFlowIncome(parseFloat(e.target.value))}
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="cf-fixed">Despesas Fixas (€)</Label>
                      <Input
                        id="cf-fixed"
                        type="number"
                        min="0"
                        step="0.01"
                        value={cashFlowFixed}
                        onChange={(e) => setCashFlowFixed(parseFloat(e.target.value))}
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="cf-variable">Despesas Variáveis (€)</Label>
                      <Input
                        id="cf-variable"
                        type="number"
                        min="0"
                        step="0.01"
                        value={cashFlowVariable}
                        onChange={(e) => setCashFlowVariable(parseFloat(e.target.value))}
                      />
                      <p className="text-xs text-muted-foreground">
                        Serão ajustadas pela inflação
                      </p>
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="cf-inflation">Taxa de Inflação Anual (%)</Label>
                      <Input
                        id="cf-inflation"
                        type="number"
                        min="0"
                        max="20"
                        step="0.1"
                        value={cashFlowInflation}
                        onChange={(e) => setCashFlowInflation(parseFloat(e.target.value))}
                      />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="cf-months">Período de Projeção (meses)</Label>
                      <Input
                        id="cf-months"
                        type="number"
                        min="1"
                        max="60"
                        value={cashFlowMonths}
                        onChange={(e) => setCashFlowMonths(parseInt(e.target.value))}
                      />
                    </div>

                    <Button onClick={handleCashFlowSimulation} className="w-full">
                      <Calculator className="w-4 h-4 mr-2" />
                      Calcular Projeção
                    </Button>
                  </div>

                  {cashFlowResult.length > 0 && (
                    <div className="space-y-4">
                      <div className="bg-primary text-white p-6 rounded-lg">
                        <p className="text-sm opacity-90 mb-2">Poupança Acumulada ({cashFlowMonths} meses)</p>
                        <p className="text-4xl">
                          €{cashFlowResult[cashFlowResult.length - 1].cumulativeSavings.toFixed(2)}
                        </p>
                      </div>

                      <div className="bg-muted p-4 rounded-lg">
                        <p className="text-sm text-muted-foreground mb-2">Análise do Último Mês</p>
                        <div className="space-y-1 text-sm">
                          <div className="flex justify-between">
                            <span>Rendimentos:</span>
                            <span>€{cashFlowResult[cashFlowResult.length - 1].income.toFixed(2)}</span>
                          </div>
                          <div className="flex justify-between">
                            <span>Despesas (ajustadas):</span>
                            <span>€{cashFlowResult[cashFlowResult.length - 1].expenses.toFixed(2)}</span>
                          </div>
                          <div className="flex justify-between font-semibold">
                            <span>Saldo Mensal:</span>
                            <span className={cashFlowResult[cashFlowResult.length - 1].netCashFlow >= 0 ? 'text-green-600' : 'text-red-600'}>
                              €{cashFlowResult[cashFlowResult.length - 1].netCashFlow.toFixed(2)}
                            </span>
                          </div>
                        </div>
                      </div>

                      <div className="bg-blue-50 border border-blue-200 p-4 rounded-lg text-sm">
                        <p>
                          <strong>Impacto da Inflação:</strong> Ao longo de {cashFlowMonths} meses, 
                          as despesas variáveis aumentaram de €{cashFlowVariable.toFixed(2)} para €
                          {cashFlowResult[cashFlowResult.length - 1].expenses.toFixed(2)}.
                        </p>
                      </div>
                    </div>
                  )}
                </div>

                {cashFlowResult.length > 0 && (
                  <div>
                    <h3 className="text-lg mb-4">Evolução do Fluxo de Caixa</h3>
                    <ResponsiveContainer width="100%" height={300}>
                      <BarChart data={cashFlowResult}>
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis dataKey="month" label={{ value: 'Mês', position: 'insideBottom', offset: -5 }} />
                        <YAxis label={{ value: 'Valor (€)', angle: -90, position: 'insideLeft' }} />
                        <Tooltip />
                        <Legend />
                        <Bar dataKey="income" name="Rendimentos" fill="#0C73B7" />
                        <Bar dataKey="expenses" name="Despesas" fill="#d4183d" />
                        <Bar dataKey="netCashFlow" name="Saldo Mensal" fill="#2da7df" />
                      </BarChart>
                    </ResponsiveContainer>

                    <h3 className="text-lg mt-6 mb-4">Poupança Acumulada</h3>
                    <ResponsiveContainer width="100%" height={300}>
                      <LineChart data={cashFlowResult}>
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis dataKey="month" label={{ value: 'Mês', position: 'insideBottom', offset: -5 }} />
                        <YAxis label={{ value: 'Poupança (€)', angle: -90, position: 'insideLeft' }} />
                        <Tooltip />
                        <Legend />
                        <Line type="monotone" dataKey="cumulativeSavings" name="Poupança Acumulada" stroke="#0C73B7" strokeWidth={3} />
                      </LineChart>
                    </ResponsiveContainer>
                  </div>
                )}
              </CardContent>
            </Card>
          </TabsContent>
        </Tabs>
      </div>
    </div>
  );
}
