import React, { useState } from "react";
import { signInWithEmailAndPassword, type UserCredential } from "firebase/auth";
import { auth } from "../firebase";
import bgImage from '../assets/worldmap.jpg';
import { useNavigate } from "react-router-dom";
import { config } from "../config";

const LogInPage: React.FC = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const goToSignUp = () => {
        navigate('/signup');
    }

    const goToGoogle = () => {
        navigate('/login/google');
    }

    const login = async () => {
        let userCredentials: UserCredential;
        try {
            userCredentials = await signInWithEmailAndPassword(auth, email, password);
            const token = await userCredentials.user.getIdToken(true);

            console.log(token);

            const response = await fetch(config.LOG_IN_URL, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
            });

            if (response.ok) {
                alert('Everything succeeded');
                navigate('/');
            } else {
                alert('Have some mistakes');
            }
        } catch (error) {
            if (error instanceof Error) {
                alert("Error: " + error.message);
            } else {
                alert("Unknown error");
            }
        }
    };
    return (
        <div style={{backgroundImage: `url(${bgImage})`, backgroundRepeat: 'space repeat'}} className="w-screen h-screen">
            <div className="h-screen w-1/3 bg-white m-auto">
                <div>
                    <label className="block m-2 font-semibold">Email</label>
                    <input placeholder="Enter your email" type="email" value={email} className="w-full border-2 p-1 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setEmail(e.target.value)}}></input>

                    <label className="block m-2 font-semibold">Password</label>
                    <input placeholder="Enter your password" type="password" value={password} className="w-full p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setPassword(e.target.value)}}></input>

                    <button className="m-2 w-30 border-2 border-gray-300" onClick={login}>Log In</button>
                </div>
                <p className="font-bold m-2 text-right">Don't have an account?</p>
                <div className="flex justify-end m-2 mt-4">
                    <button className="w-30 border-2 border-gray-300" onClick={goToSignUp}>Sign Up</button>
                </div>
                <div className="justify-end m-2 mt-4 flex">
                    <button className="w-48 flex border-2 border-gray-300" onClick={goToGoogle}>Log In With Gooogle</button>
                </div>
                
            </div>
        </div>);
}

export default LogInPage;