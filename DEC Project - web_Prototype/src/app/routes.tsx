import { createBrowserRouter } from "react-router";
import { Root } from "./components/Root";
import { LandingPage } from "./components/LandingPage";
import { Login } from "./components/Login";
import { Register } from "./components/Register";
import { StudentDashboard } from "./components/StudentDashboard";
import { FamilyScenario } from "./components/FamilyScenario";
import { Transactions } from "./components/Transactions";
import { Simulations } from "./components/Simulations";
import { TeacherDashboard } from "./components/TeacherDashboard";
import { Challenges } from "./components/Challenges";
import { AdminDashboard } from "./components/AdminDashboard";
import { NotFound } from "./components/NotFound";

export const router = createBrowserRouter([
  {
    path: "/",
    Component: Root,
    children: [
      { index: true, Component: LandingPage },
      { path: "login", Component: Login },
      { path: "register", Component: Register },
      { path: "dashboard", Component: StudentDashboard },
      { path: "family", Component: FamilyScenario },
      { path: "transactions", Component: Transactions },
      { path: "simulations", Component: Simulations },
      { path: "teacher", Component: TeacherDashboard },
      { path: "challenges", Component: Challenges },
      { path: "admin", Component: AdminDashboard },
      { path: "*", Component: NotFound },
    ],
  },
]);
