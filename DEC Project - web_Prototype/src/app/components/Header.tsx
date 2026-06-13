import { Link, useNavigate } from 'react-router';
import { useApp } from '../context/AppContext';
import { Button } from './ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from './ui/dropdown-menu';
import { User, LogOut, Home, FileText, TrendingUp, Target, GraduationCap, Settings } from 'lucide-react';
import logoWhite from 'figma:asset/0adf42d423b7a187f0e52c3f8865e6194626553e.png';
import logoColor from 'figma:asset/15b23ae75b11299058584f1907ef93163ec08d27.png';

interface HeaderProps {
  variant?: 'light' | 'dark';
}

export function Header({ variant = 'dark' }: HeaderProps) {
  const { user, setUser } = useApp();
  const navigate = useNavigate();

  const handleLogout = () => {
    setUser(null);
    navigate('/');
  };

  const logo = variant === 'light' ? logoWhite : logoColor;

  return (
    <header className={`${variant === 'light' ? 'bg-primary text-white' : 'bg-white border-b'} py-4 px-6`}>
      <div className="max-w-7xl mx-auto flex items-center justify-between">
        <Link to="/" className="flex items-center gap-3">
          <img src={logo} alt="ISTEC" className="h-10" />
          <div className="border-l border-current/20 pl-3">
            <h1 className={`text-xl font-bold ${variant === 'light' ? 'text-white' : 'text-primary'}`}>
              DEC – Dinheiro em Casa
            </h1>
          </div>
        </Link>

        {user && (
          <nav className="flex items-center gap-4">
            {user.role === 'student' && (
              <>
                <Link to="/dashboard">
                  <Button variant="ghost" size="sm" className={variant === 'light' ? 'text-white hover:bg-white/10' : ''}>
                    <Home className="w-4 h-4 mr-2" />
                    Dashboard
                  </Button>
                </Link>
                <Link to="/family">
                  <Button variant="ghost" size="sm" className={variant === 'light' ? 'text-white hover:bg-white/10' : ''}>
                    <User className="w-4 h-4 mr-2" />
                    Família
                  </Button>
                </Link>
                <Link to="/transactions">
                  <Button variant="ghost" size="sm" className={variant === 'light' ? 'text-white hover:bg-white/10' : ''}>
                    <FileText className="w-4 h-4 mr-2" />
                    Lançamentos
                  </Button>
                </Link>
                <Link to="/simulations">
                  <Button variant="ghost" size="sm" className={variant === 'light' ? 'text-white hover:bg-white/10' : ''}>
                    <TrendingUp className="w-4 h-4 mr-2" />
                    Simulações
                  </Button>
                </Link>
                <Link to="/challenges">
                  <Button variant="ghost" size="sm" className={variant === 'light' ? 'text-white hover:bg-white/10' : ''}>
                    <Target className="w-4 h-4 mr-2" />
                    Desafios
                  </Button>
                </Link>
              </>
            )}
            {user.role === 'teacher' && (
              <>
                <Link to="/teacher">
                  <Button variant="ghost" size="sm" className={variant === 'light' ? 'text-white hover:bg-white/10' : ''}>
                    <GraduationCap className="w-4 h-4 mr-2" />
                    Turmas
                  </Button>
                </Link>
                <Link to="/challenges">
                  <Button variant="ghost" size="sm" className={variant === 'light' ? 'text-white hover:bg-white/10' : ''}>
                    <Target className="w-4 h-4 mr-2" />
                    Desafios
                  </Button>
                </Link>
              </>
            )}
            {user.role === 'admin' && (
              <Link to="/admin">
                <Button variant="ghost" size="sm" className={variant === 'light' ? 'text-white hover:bg-white/10' : ''}>
                  <Settings className="w-4 h-4 mr-2" />
                  Administração
                </Button>
              </Link>
            )}

            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button variant="ghost" size="sm" className={variant === 'light' ? 'text-white hover:bg-white/10' : ''}>
                  <User className="w-4 h-4 mr-2" />
                  {user.name}
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end">
                <DropdownMenuLabel>Minha Conta</DropdownMenuLabel>
                <DropdownMenuSeparator />
                <DropdownMenuItem onClick={handleLogout}>
                  <LogOut className="w-4 h-4 mr-2" />
                  Sair
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          </nav>
        )}

        {!user && (
          <div className="flex gap-2">
            <Link to="/login">
              <Button variant={variant === 'light' ? 'secondary' : 'outline'}>
                Entrar
              </Button>
            </Link>
            <Link to="/register">
              <Button variant={variant === 'light' ? 'secondary' : 'default'}>
                Registar
              </Button>
            </Link>
          </div>
        )}
      </div>
    </header>
  );
}
