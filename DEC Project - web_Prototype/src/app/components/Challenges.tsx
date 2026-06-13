import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router';
import { useApp } from '../context/AppContext';
import { Header } from './Header';
import { Button } from './ui/button';
import { Input } from './ui/input';
import { Label } from './ui/label';
import { Textarea } from './ui/textarea';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './ui/card';
import { Badge } from './ui/badge';
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from './ui/dialog';
import { Tabs, TabsContent, TabsList, TabsTrigger } from './ui/tabs';
import { toast } from 'sonner';
import { Target, Trophy, Calendar, Award, Plus, CheckCircle2, Clock } from 'lucide-react';

export function Challenges() {
  const { user, challenges, setChallenges, userProgress, setUserProgress } = useApp();
  const navigate = useNavigate();

  const [newChallengeTitle, setNewChallengeTitle] = useState('');
  const [newChallengeDescription, setNewChallengeDescription] = useState('');
  const [newChallengeDueDate, setNewChallengeDueDate] = useState('');
  const [newChallengePoints, setNewChallengePoints] = useState(100);
  const [newChallengeTarget, setNewChallengeTarget] = useState(0);
  const [dialogOpen, setDialogOpen] = useState(false);

  useEffect(() => {
    if (!user) {
      navigate('/login');
      return;
    }

    // Initialize with mock challenges if empty
    if (challenges.length === 0) {
      const mockChallenges = [
        {
          id: '1',
          title: 'Fundo de Emergência',
          description: 'Constitui um fundo de emergência equivalente a 3 meses de despesas',
          teacherId: 'teacher1',
          teacherName: 'Prof. João Santos',
          dueDate: '2026-05-30',
          targetAmount: 3600,
          completed: false,
          points: 200,
        },
        {
          id: '2',
          title: 'Redução de Despesas',
          description: 'Reduz as tuas despesas variáveis em 15% durante 3 meses consecutivos',
          teacherId: 'teacher1',
          teacherName: 'Prof. João Santos',
          dueDate: '2026-04-30',
          completed: false,
          points: 150,
        },
        {
          id: '3',
          title: 'Meta de Poupança',
          description: 'Atinge o teu objetivo de poupança antes do prazo definido',
          teacherId: 'teacher2',
          teacherName: 'Prof. Maria Silva',
          dueDate: '2026-06-30',
          completed: false,
          points: 250,
        },
      ];
      setChallenges(mockChallenges);
    }
  }, [user, navigate, challenges.length, setChallenges]);

  const handleCreateChallenge = () => {
    if (!newChallengeTitle || !newChallengeDescription || !newChallengeDueDate) {
      toast.error('Preenche todos os campos obrigatórios');
      return;
    }

    const newChallenge = {
      id: Date.now().toString(),
      title: newChallengeTitle,
      description: newChallengeDescription,
      teacherId: user!.id,
      teacherName: user!.name,
      dueDate: newChallengeDueDate,
      targetAmount: newChallengeTarget > 0 ? newChallengeTarget : undefined,
      completed: false,
      points: newChallengePoints,
    };

    setChallenges([...challenges, newChallenge]);
    toast.success('Desafio criado com sucesso!');
    
    // Reset form
    setNewChallengeTitle('');
    setNewChallengeDescription('');
    setNewChallengeDueDate('');
    setNewChallengePoints(100);
    setNewChallengeTarget(0);
    setDialogOpen(false);
  };

  const handleCompleteChallenge = (challengeId: string) => {
    const challenge = challenges.find(c => c.id === challengeId);
    if (!challenge) return;

    const updatedChallenges = challenges.map(c =>
      c.id === challengeId ? { ...c, completed: true } : c
    );
    setChallenges(updatedChallenges);

    // Award badge
    const badges = [...userProgress.badges];
    if (challenge.points >= 200 && !badges.includes('🏆 Super Poupador')) {
      badges.push('🏆 Super Poupador');
    }
    if (challenge.title.includes('Emergência') && !badges.includes('🛡️ Previdente')) {
      badges.push('🛡️ Previdente');
    }

    setUserProgress({
      ...userProgress,
      badges,
      completedChallenges: userProgress.completedChallenges + 1,
    });

    toast.success(`Desafio concluído! +${challenge.points} pontos`);
  };

  if (!user) {
    return null;
  }

  const activeChallenges = challenges.filter(c => !c.completed);
  const completedChallenges = challenges.filter(c => c.completed);
  const isTeacher = user.role === 'teacher';

  return (
    <div className="min-h-screen bg-gray-50">
      <Header />
      
      <div className="max-w-6xl mx-auto p-6">
        <div className="flex items-center justify-between mb-6">
          <div>
            <h1 className="text-3xl mb-2">Desafios Financeiros</h1>
            <p className="text-muted-foreground">
              {isTeacher ? 'Cria e gere desafios para os teus alunos' : 'Completa desafios e ganha badges'}
            </p>
          </div>
          
          {isTeacher && (
            <Dialog open={dialogOpen} onOpenChange={setDialogOpen}>
              <DialogTrigger asChild>
                <Button>
                  <Plus className="w-4 h-4 mr-2" />
                  Criar Desafio
                </Button>
              </DialogTrigger>
              <DialogContent className="max-w-2xl">
                <DialogHeader>
                  <DialogTitle>Criar Novo Desafio</DialogTitle>
                  <DialogDescription>
                    Define um desafio pedagógico para os teus alunos
                  </DialogDescription>
                </DialogHeader>
                <div className="space-y-4">
                  <div className="space-y-2">
                    <Label htmlFor="title">Título do Desafio</Label>
                    <Input
                      id="title"
                      placeholder="ex: Fundo de Emergência"
                      value={newChallengeTitle}
                      onChange={(e) => setNewChallengeTitle(e.target.value)}
                    />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="description">Descrição</Label>
                    <Textarea
                      id="description"
                      placeholder="Descreve o objetivo do desafio..."
                      value={newChallengeDescription}
                      onChange={(e) => setNewChallengeDescription(e.target.value)}
                      rows={4}
                    />
                  </div>
                  <div className="grid md:grid-cols-2 gap-4">
                    <div className="space-y-2">
                      <Label htmlFor="due-date">Data Limite</Label>
                      <Input
                        id="due-date"
                        type="date"
                        value={newChallengeDueDate}
                        onChange={(e) => setNewChallengeDueDate(e.target.value)}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label htmlFor="points">Pontos</Label>
                      <Input
                        id="points"
                        type="number"
                        min="0"
                        value={newChallengePoints}
                        onChange={(e) => setNewChallengePoints(parseInt(e.target.value))}
                      />
                    </div>
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="target">Valor Alvo (€) (opcional)</Label>
                    <Input
                      id="target"
                      type="number"
                      min="0"
                      value={newChallengeTarget}
                      onChange={(e) => setNewChallengeTarget(parseFloat(e.target.value))}
                    />
                  </div>
                </div>
                <DialogFooter>
                  <Button variant="outline" onClick={() => setDialogOpen(false)}>
                    Cancelar
                  </Button>
                  <Button onClick={handleCreateChallenge}>
                    Criar Desafio
                  </Button>
                </DialogFooter>
              </DialogContent>
            </Dialog>
          )}
        </div>

        {!isTeacher && (
          <div className="grid md:grid-cols-3 gap-4 mb-8">
            <Card>
              <CardHeader className="pb-2">
                <CardDescription>Desafios Ativos</CardDescription>
                <CardTitle className="text-3xl flex items-center gap-2">
                  <Clock className="w-6 h-6 text-primary" />
                  {activeChallenges.length}
                </CardTitle>
              </CardHeader>
            </Card>

            <Card>
              <CardHeader className="pb-2">
                <CardDescription>Desafios Concluídos</CardDescription>
                <CardTitle className="text-3xl flex items-center gap-2">
                  <CheckCircle2 className="w-6 h-6 text-green-500" />
                  {completedChallenges.length}
                </CardTitle>
              </CardHeader>
            </Card>

            <Card>
              <CardHeader className="pb-2">
                <CardDescription>Badges Conquistadas</CardDescription>
                <CardTitle className="text-3xl flex items-center gap-2">
                  <Trophy className="w-6 h-6 text-yellow-500" />
                  {userProgress.badges.length}
                </CardTitle>
              </CardHeader>
            </Card>
          </div>
        )}

        <Tabs defaultValue="active" className="space-y-6">
          <TabsList>
            <TabsTrigger value="active">
              Desafios Ativos ({activeChallenges.length})
            </TabsTrigger>
            <TabsTrigger value="completed">
              Concluídos ({completedChallenges.length})
            </TabsTrigger>
            {!isTeacher && (
              <TabsTrigger value="badges">
                Minhas Badges ({userProgress.badges.length})
              </TabsTrigger>
            )}
          </TabsList>

          <TabsContent value="active" className="space-y-4">
            {activeChallenges.length === 0 ? (
              <Card>
                <CardContent className="text-center py-12">
                  <Target className="w-16 h-16 mx-auto mb-4 text-muted-foreground" />
                  <p className="text-muted-foreground">
                    {isTeacher ? 'Nenhum desafio criado ainda' : 'Nenhum desafio ativo no momento'}
                  </p>
                </CardContent>
              </Card>
            ) : (
              activeChallenges.map((challenge) => (
                <Card key={challenge.id} className="hover:shadow-md transition-shadow">
                  <CardHeader>
                    <div className="flex items-start justify-between">
                      <div className="flex-1">
                        <CardTitle className="flex items-center gap-2 mb-2">
                          <Target className="w-5 h-5 text-primary" />
                          {challenge.title}
                        </CardTitle>
                        <CardDescription>{challenge.description}</CardDescription>
                      </div>
                      <Badge variant="secondary" className="ml-4">
                        <Award className="w-3 h-3 mr-1" />
                        {challenge.points} pts
                      </Badge>
                    </div>
                  </CardHeader>
                  <CardContent>
                    <div className="flex items-center justify-between">
                      <div className="flex items-center gap-4 text-sm text-muted-foreground">
                        <div className="flex items-center gap-1">
                          <Calendar className="w-4 h-4" />
                          {new Date(challenge.dueDate).toLocaleDateString('pt-PT')}
                        </div>
                        {challenge.targetAmount && (
                          <div className="flex items-center gap-1">
                            <Target className="w-4 h-4" />
                            Meta: €{challenge.targetAmount}
                          </div>
                        )}
                        {isTeacher && (
                          <div className="text-xs">
                            Criado por: {challenge.teacherName}
                          </div>
                        )}
                      </div>
                      {!isTeacher && (
                        <Button
                          size="sm"
                          onClick={() => handleCompleteChallenge(challenge.id)}
                        >
                          <CheckCircle2 className="w-4 h-4 mr-2" />
                          Marcar como Concluído
                        </Button>
                      )}
                    </div>
                  </CardContent>
                </Card>
              ))
            )}
          </TabsContent>

          <TabsContent value="completed" className="space-y-4">
            {completedChallenges.length === 0 ? (
              <Card>
                <CardContent className="text-center py-12">
                  <Trophy className="w-16 h-16 mx-auto mb-4 text-muted-foreground" />
                  <p className="text-muted-foreground">Nenhum desafio concluído ainda</p>
                </CardContent>
              </Card>
            ) : (
              completedChallenges.map((challenge) => (
                <Card key={challenge.id} className="border-green-200 bg-green-50/50">
                  <CardHeader>
                    <div className="flex items-start justify-between">
                      <div className="flex-1">
                        <CardTitle className="flex items-center gap-2 mb-2">
                          <CheckCircle2 className="w-5 h-5 text-green-600" />
                          {challenge.title}
                        </CardTitle>
                        <CardDescription>{challenge.description}</CardDescription>
                      </div>
                      <Badge className="ml-4 bg-green-600">
                        <Trophy className="w-3 h-3 mr-1" />
                        Concluído
                      </Badge>
                    </div>
                  </CardHeader>
                  <CardContent>
                    <div className="flex items-center gap-4 text-sm text-muted-foreground">
                      <div className="flex items-center gap-1">
                        <Award className="w-4 h-4" />
                        +{challenge.points} pontos
                      </div>
                      {challenge.targetAmount && (
                        <div>Meta: €{challenge.targetAmount}</div>
                      )}
                    </div>
                  </CardContent>
                </Card>
              ))
            )}
          </TabsContent>

          {!isTeacher && (
            <TabsContent value="badges">
              <Card>
                <CardHeader>
                  <CardTitle>Minhas Conquistas</CardTitle>
                  <CardDescription>
                    Badges ganhas ao completar desafios
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  {userProgress.badges.length === 0 ? (
                    <div className="text-center py-12">
                      <Award className="w-16 h-16 mx-auto mb-4 text-muted-foreground" />
                      <p className="text-muted-foreground">
                        Completa desafios para ganhar badges!
                      </p>
                    </div>
                  ) : (
                    <div className="grid md:grid-cols-3 gap-4">
                      {userProgress.badges.map((badge, index) => (
                        <div
                          key={index}
                          className="bg-gradient-to-br from-primary/10 to-secondary/10 p-6 rounded-lg text-center border-2 border-primary/20"
                        >
                          <div className="text-5xl mb-2">{badge.split(' ')[0]}</div>
                          <p className="font-medium">{badge.split(' ').slice(1).join(' ')}</p>
                        </div>
                      ))}
                    </div>
                  )}
                </CardContent>
              </Card>
            </TabsContent>
          )}
        </Tabs>
      </div>
    </div>
  );
}
