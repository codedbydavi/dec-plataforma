import { Link } from 'react-router';
import { Header } from './Header';
import { Button } from './ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './ui/card';
import { TrendingUp, Target, PiggyBank, GraduationCap, BarChart3, Calculator } from 'lucide-react';
import logoFull from 'figma:asset/d5218954818f0b46d89a04972d9ea1b2b5f28bba.png';

export function LandingPage() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-white via-blue-50 to-blue-100">
      <Header variant="light" />
      
      {/* Hero Section */}
      <section className="bg-primary text-white py-20 px-6">
        <div className="max-w-6xl mx-auto text-center">
          <img src={logoFull} alt="ISTEC Logo" className="h-20 mx-auto mb-6" />
          <h1 className="text-5xl mb-6" style={{ fontFamily: 'Roboto, sans-serif' }}>
            DEC – Dinheiro em Casa
          </h1>
          <p className="text-xl mb-8 max-w-3xl mx-auto opacity-90">
            Plataforma digital de literacia financeira para alunos do 10.º ao 12.º ano. 
            Aprende a gerir a economia doméstica através de cenários reais e simulações interativas.
          </p>
          <div className="flex gap-4 justify-center">
            <Link to="/register">
              <Button size="lg" variant="secondary" className="text-lg px-8">
                Começar Agora
              </Button>
            </Link>
            <Link to="/login">
              <Button size="lg" variant="outline" className="text-lg px-8 bg-white/10 text-white border-white hover:bg-white/20">
                Já tenho conta
              </Button>
            </Link>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section className="py-16 px-6">
        <div className="max-w-6xl mx-auto">
          <h2 className="text-4xl text-center mb-12 text-primary" style={{ fontFamily: 'Roboto, sans-serif' }}>
            Funcionalidades Principais
          </h2>
          <div className="grid md:grid-cols-3 gap-6">
            <Card className="border-2 hover:border-primary transition-colors">
              <CardHeader>
                <div className="w-12 h-12 bg-primary/10 rounded-lg flex items-center justify-center mb-4">
                  <Target className="w-6 h-6 text-primary" />
                </div>
                <CardTitle>Cenários Familiares</CardTitle>
                <CardDescription>
                  Cria famílias virtuais e toma decisões financeiras reais
                </CardDescription>
              </CardHeader>
            </Card>

            <Card className="border-2 hover:border-primary transition-colors">
              <CardHeader>
                <div className="w-12 h-12 bg-secondary/10 rounded-lg flex items-center justify-center mb-4">
                  <PiggyBank className="w-6 h-6 text-secondary" />
                </div>
                <CardTitle>Gestão de Poupança</CardTitle>
                <CardDescription>
                  Define objetivos e simula o crescimento da tua poupança com juros compostos
                </CardDescription>
              </CardHeader>
            </Card>

            <Card className="border-2 hover:border-primary transition-colors">
              <CardHeader>
                <div className="w-12 h-12 bg-primary/10 rounded-lg flex items-center justify-center mb-4">
                  <Calculator className="w-6 h-6 text-primary" />
                </div>
                <CardTitle>Simulador de Crédito</CardTitle>
                <CardDescription>
                  Simula planos de amortização e compreende o impacto dos juros
                </CardDescription>
              </CardHeader>
            </Card>

            <Card className="border-2 hover:border-primary transition-colors">
              <CardHeader>
                <div className="w-12 h-12 bg-secondary/10 rounded-lg flex items-center justify-center mb-4">
                  <BarChart3 className="w-6 h-6 text-secondary" />
                </div>
                <CardTitle>Dashboard Interativo</CardTitle>
                <CardDescription>
                  Visualiza graficamente os teus saldos, taxa de esforço e evolução
                </CardDescription>
              </CardHeader>
            </Card>

            <Card className="border-2 hover:border-primary transition-colors">
              <CardHeader>
                <div className="w-12 h-12 bg-primary/10 rounded-lg flex items-center justify-center mb-4">
                  <TrendingUp className="w-6 h-6 text-primary" />
                </div>
                <CardTitle>Projeções Financeiras</CardTitle>
                <CardDescription>
                  Relatórios de projeção a 12/24 meses considerando inflação
                </CardDescription>
              </CardHeader>
            </Card>

            <Card className="border-2 hover:border-primary transition-colors">
              <CardHeader>
                <div className="w-12 h-12 bg-secondary/10 rounded-lg flex items-center justify-center mb-4">
                  <GraduationCap className="w-6 h-6 text-secondary" />
                </div>
                <CardTitle>Desafios Pedagógicos</CardTitle>
                <CardDescription>
                  Completa desafios propostos pelos professores e ganha badges
                </CardDescription>
              </CardHeader>
            </Card>
          </div>
        </div>
      </section>

      {/* Benefits Section */}
      <section className="bg-white py-16 px-6">
        <div className="max-w-4xl mx-auto">
          <h2 className="text-4xl text-center mb-12 text-primary" style={{ fontFamily: 'Roboto, sans-serif' }}>
            Porquê DEC – Dinheiro em Casa?
          </h2>
          <div className="space-y-6">
            <div className="flex gap-4">
              <div className="w-2 bg-primary rounded-full"></div>
              <div>
                <h3 className="text-xl mb-2">Aprendizagem Prática</h3>
                <p className="text-muted-foreground">
                  Simula cenários reais de gestão doméstica e observa o impacto das tuas decisões a curto e longo prazo
                </p>
              </div>
            </div>
            <div className="flex gap-4">
              <div className="w-2 bg-secondary rounded-full"></div>
              <div>
                <h3 className="text-xl mb-2">Feedback Automático</h3>
                <p className="text-muted-foreground">
                  Recebe feedback sobre o cumprimento de metas pedagógicas e melhora continuamente
                </p>
              </div>
            </div>
            <div className="flex gap-4">
              <div className="w-2 bg-primary rounded-full"></div>
              <div>
                <h3 className="text-xl mb-2">Acompanhamento Docente</h3>
                <p className="text-muted-foreground">
                  Os professores podem criar desafios personalizados e monitorizar o progresso da turma
                </p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="bg-primary text-white py-16 px-6">
        <div className="max-w-4xl mx-auto text-center">
          <h2 className="text-4xl mb-6" style={{ fontFamily: 'Roboto, sans-serif' }}>
            Pronto para começar?
          </h2>
          <p className="text-xl mb-8 opacity-90">
            Cria a tua conta gratuita e começa a aprender literacia financeira hoje mesmo
          </p>
          <Link to="/register">
            <Button size="lg" variant="secondary" className="text-lg px-8">
              Criar Conta Gratuita
            </Button>
          </Link>
        </div>
      </section>

      {/* Footer */}
      <footer className="bg-[#1d1d1b] text-white py-8 px-6">
        <div className="max-w-6xl mx-auto text-center">
          <p className="opacity-80">
            © 2026 ISTEC – Instituto Superior de Tecnologias Avançadas do Porto
          </p>
          <p className="text-sm opacity-60 mt-2">
            Plataforma educativa para literacia financeira
          </p>
        </div>
      </footer>
    </div>
  );
}
