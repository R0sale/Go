import MainPage from "./pages/MainPage";
import SignUpPage from "./pages/SignUpPage";
import LogInPage from "./pages/LogInPage";
import { Routes, Route } from "react-router-dom";
import GoogleSignInPage from "./pages/GoogleSignInPage";
import UserPage from "./pages/UserPage";
import FacilityPage from "./pages/FacilityPage";
import CreateFacilityPage from "./pages/CreateFacilityPage";
import AdminPage from "./pages/AdminPage";
import UserManagingPage from "./pages/UserManagingPage";
import Images from "./pages/Images";
import RoutesPage from "./pages/RoutesPage";
import TransportPage from "./pages/TransportPage";

function App() {
  return (
      <Routes>
        <Route path="/" element={<MainPage />} />
        <Route path="/signup" element={<SignUpPage />} />
        <Route path="/login" element={<LogInPage />} />
        <Route path="/login/google" element={<GoogleSignInPage />} />
        <Route path="/userPage" element={<UserPage />} />
        <Route path="/userPage/facilityPage/:facilityId" element={<FacilityPage />}/>
        <Route path="/userPage/facilityPage" element={<CreateFacilityPage />}/>
        <Route path="/adminPage" element={<AdminPage />} />
        <Route path="/adminPage/user/:userUid" element={<UserManagingPage />} />
        <Route path="/images" element={<Images />} />
        <Route path="/routes" element={<RoutesPage />} />
        <Route path="/userPage/transport" element={<TransportPage />} />
      </Routes>
  );
}

export default App;
