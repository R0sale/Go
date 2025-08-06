import React, { useState } from "react";
import { signInWithEmailAndPassword } from "firebase/auth";
import { auth } from "../firebase";
import bgImage from '../assets/worldmap.jpg';

const SignInPage: React.FC = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const login = async () => {
        try {
            const userCredentials = await signInWithEmailAndPassword(auth, email, password);
            const token = await userCredentials.user.getIdToken();

            const response = await fetch('', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`
                },
            });

            if (response.ok) {
                alert('Everything succeeded');
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
                    <input placeholder="Enter your email" type="email" value={email} className="w-inp border-2 p-1 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setEmail(e.target.value)}}></input>

                    <label className="block m-2 font-semibold">Password</label>
                    <input placeholder="Enter your password" type="password" value={password} className="w-inp p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setPassword(e.target.value)}}></input>

                    <label className="block m-2 font-semibold">Repeat password</label>
                    <input placeholder="Repeat your password" type="password" value={password} className="w-inp p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setPassword(e.target.value)}}></input>

                    <button className="m-2 w-24 border-2 border-gray-300" onClick={login}>Sign In</button>
                </div>
                <p className="font-bold m-2 text-right">Already have an account?</p>
                <div className="flex justify-end m-2 mt-4">
                    <button className="w-24 border-2 border-gray-300">Log In</button>
                </div>
            </div>
        </div>);
}

export default SignInPage;