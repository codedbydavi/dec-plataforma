import React, { createContext, useContext, useState, ReactNode } from 'react';

export type UserRole = 'student' | 'teacher' | 'admin';

export interface User {
  id: string;
  name: string;
  email: string;
  role: UserRole;
}

export interface FamilyScenario {
  id: string;
  name: string;
  location: string;
  members: number;
  monthlyIncome: number;
  fixedExpenses: number;
  variableExpenses: number;
  savingsGoal: number;
  savingsDeadline: number;
  currentSavings: number;
}

export interface Transaction {
  id: string;
  scenarioId: string;
  type: 'income' | 'expense';
  category: string;
  amount: number;
  description: string;
  date: string;
  isRecurring: boolean;
}

export interface Challenge {
  id: string;
  title: string;
  description: string;
  teacherId: string;
  teacherName: string;
  dueDate: string;
  targetAmount?: number;
  completed: boolean;
  points: number;
}

interface AppContextType {
  user: User | null;
  setUser: (user: User | null) => void;
  families: FamilyScenario[];
  setFamilies: (families: FamilyScenario[]) => void;
  currentFamily: FamilyScenario | null;
  setCurrentFamily: (family: FamilyScenario | null) => void;
  transactions: Transaction[];
  setTransactions: (transactions: Transaction[]) => void;
  challenges: Challenge[];
  setChallenges: (challenges: Challenge[]) => void;
  userProgress: {
    badges: string[];
    completedChallenges: number;
    totalSavings: number;
  };
  setUserProgress: (progress: any) => void;
}

const AppContext = createContext<AppContextType | undefined>(undefined);

export const AppProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [families, setFamilies] = useState<FamilyScenario[]>([]);
  const [currentFamily, setCurrentFamily] = useState<FamilyScenario | null>(null);
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [challenges, setChallenges] = useState<Challenge[]>([
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
  ]);
  const [userProgress, setUserProgress] = useState({
    badges: [],
    completedChallenges: 0,
    totalSavings: 0,
  });

  return (
    <AppContext.Provider
      value={{
        user,
        setUser,
        families,
        setFamilies,
        currentFamily,
        setCurrentFamily,
        transactions,
        setTransactions,
        challenges,
        setChallenges,
        userProgress,
        setUserProgress,
      }}
    >
      {children}
    </AppContext.Provider>
  );
};

export const useApp = () => {
  const context = useContext(AppContext);
  if (context === undefined) {
    throw new Error('useApp must be used within an AppProvider');
  }
  return context;
};