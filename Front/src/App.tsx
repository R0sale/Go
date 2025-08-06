import MainPage from "./pages/MainPage";
import SignInPage from "./pages/SignInPage";
import { Routes, Route } from "react-router-dom";

function App() {
  return (
      <Routes>
        <Route path="/" element={<MainPage />} />
        <Route path="/login" element={<SignInPage />} />
      </Routes>
  );
}

export default App;
