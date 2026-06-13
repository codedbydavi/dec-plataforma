import { Outlet } from 'react-router';
import { AppProvider } from '../context/AppContext';
import { Toaster } from './ui/sonner';

export function Root() {
  return (
    <AppProvider>
      <div className="min-h-screen">
        <Outlet />
        <Toaster />
      </div>
    </AppProvider>
  );
}
