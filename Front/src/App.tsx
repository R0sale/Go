import MainPage from "./pages/MainPage";
import SignUpPage from "./pages/SignUpPage";
import LogInPage from "./pages/LogInPage";
import { Routes, Route } from "react-router-dom";
import GoogleSignInPage from "./pages/GoogleSignInPage";

function App() {
  return (
      <Routes>
        <Route path="/" element={<MainPage />} />
        <Route path="/signup" element={<SignUpPage />} />
        <Route path="/login" element={<LogInPage />} />
        <Route path="/login/google" element={<GoogleSignInPage />} />
      </Routes>
  );
}

export default App;
