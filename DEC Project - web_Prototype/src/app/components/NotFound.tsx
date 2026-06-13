import { Link } from 'react-router';
import { Button } from './ui/button';
import { Home, ArrowLeft } from 'lucide-react';

export function NotFound() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-primary via-secondary to-primary flex items-center justify-center p-6">
      <div className="text-center text-white">
        <h1 className="text-9xl mb-4" style={{ fontFamily: 'Roboto, sans-serif' }}>404</h1>
        <h2 className="text-3xl mb-4">Página Não Encontrada</h2>
        <p className="text-xl mb-8 opacity-90 max-w-md mx-auto">
          A página que procuras não existe ou foi movida.
        </p>
        <div className="flex gap-4 justify-center">
          <Link to="/">
            <Button size="lg" variant="secondary">
              <Home className="w-4 h-4 mr-2" />
              Ir para a Página Inicial
            </Button>
          </Link>
          <Button size="lg" variant="outline" className="bg-white/10 text-white border-white hover:bg-white/20" onClick={() => window.history.back()}>
            <ArrowLeft className="w-4 h-4 mr-2" />
            Voltar
          </Button>
        </div>
      </div>
    </div>
  );
}
