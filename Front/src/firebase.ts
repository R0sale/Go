import { initializeApp } from "firebase/app";
import { getAuth, GoogleAuthProvider } from "firebase/auth";

const firebaseConfig = {
  apiKey: "AIzaSyCy1mUmZNki8_KNtnIwHrDF6e5mzcpjN0g",
  authDomain: "authproj-b6bdf.firebaseapp.com",
  projectId: "authproj-b6bdf",
  storageBucket: "authproj-b6bdf.firebasestorage.app",
  messagingSenderId: "167331942870",
  appId: "1:167331942870:web:51bcd45dc1a339f77a1477"
};

const app = initializeApp(firebaseConfig);
export const auth = getAuth(app);

export const googleProvider = new GoogleAuthProvider();