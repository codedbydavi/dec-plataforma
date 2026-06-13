import { useState } from 'react';
import { useNavigate, Link } from 'react-router';
import { useApp } from '../context/AppContext';
import { Button } from './ui/button';
import { Input } from './ui/input';
import { Label } from './ui/label';
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from './ui/card';
import { toast } from 'sonner';
import logoColor from 'figma:asset/15b23ae75b11299058584f1907ef93163ec08d27.png';

export function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const { setUser } = useApp();
  const navigate = useNavigate();

  const handleLogin = (e: React.FormEvent) => {
    e.preventDefault();

    // Mock authentication
    if (email && password) {
      const mockUser = {
        id: '1',
        name: email.split('@')[0],
        email: email,
        role: email.includes('professor') ? 'teacher' as const : 
              email.includes('admin') ? 'admin' as const : 
              'student' as const,
      };

      setUser(mockUser);
      toast.success('Login efetuado com sucesso!');
      
      if (mockUser.role === 'teacher') {
        navigate('/teacher');
      } else if (mockUser.role === 'admin') {
        navigate('/admin');
      } else {
        navigate('/dashboard');
      }
    } else {
      toast.error('Por favor, preencha todos os campos');
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-primary via-secondary to-primary flex items-center justify-center p-6">
      <Card className="w-full max-w-md">
        <CardHeader className="text-center">
          <img src={logoColor} alt="ISTEC" className="h-12 mx-auto mb-4" />
          <CardTitle className="text-2xl">Entrar</CardTitle>
          <CardDescription>
            Acede à plataforma DEC – Dinheiro em Casa
          </CardDescription>
        </CardHeader>
        <form onSubmit={handleLogin}>
          <CardContent className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="email">Email</Label>
              <Input
                id="email"
                type="email"
                placeholder="exemplo@istec.pt"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="password">Password</Label>
              <Input
                id="password"
                type="password"
                placeholder="••••••••"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </div>
            <div className="text-sm text-muted-foreground">
              <p className="mb-2">Utilizadores de teste:</p>
              <ul className="text-xs space-y-1">
                <li>• aluno@istec.pt (Aluno)</li>
                <li>• professor@istec.pt (Professor)</li>
                <li>• admin@istec.pt (Administrador)</li>
              </ul>
            </div>
          </CardContent>
          <CardFooter className="flex flex-col gap-4">
            <Button type="submit" className="w-full">
              Entrar
            </Button>
            <div className="text-sm text-center">
              Não tens conta?{' '}
              <Link to="/register" className="text-primary hover:underline">
                Registar
              </Link>
            </div>
          </CardFooter>
        </form>
      </Card>
    </div>
  );
}
