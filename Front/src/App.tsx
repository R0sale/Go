import MainPage from "./pages/MainPage";
import SignUpPage from "./pages/SignUpPage";
import LogInPage from "./pages/LoginPage";
import { Routes, Route } from "react-router-dom";

function App() {
  return (
      <Routes>
        <Route path="/" element={<MainPage />} />
        <Route path="/signup" element={<SignUpPage />} />
        <Route path="/login" element={<LogInPage />} />
      </Routes>
  );
}

export default App;
