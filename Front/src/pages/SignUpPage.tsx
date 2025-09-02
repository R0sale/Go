import React, { useState } from "react";
import { createUserWithEmailAndPassword, deleteUser, type UserCredential } from "firebase/auth";
import { auth } from "../firebase";
import bgImage from '../assets/worldmap.jpg';
import { useNavigate } from "react-router-dom";
import { config } from "../config";
import * as z from "zod";

const SignUpPage: React.FC = () => {
    const [email, setEmail] = useState('');
    const [userName, setUserName] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [password, setPassword] = useState('');
    const [repeatedPassword, setRepeatedPassword] = useState('');
    const navigate = useNavigate();

    const goToLogIn = () => {
                navigate('/login');
            };

    const LogInWithGoogle = async () => {
        navigate('/login/google');
    }

    const validate = (user: object) => {
        const User = z.object({
            email: z.email(),
            firstname: z.string().min(4, 'Firstname must be at least 4 characters').max(20, 'Firstname must be at most 20 characters'),
            lastname: z.string().min(4, 'Lastname must be at least 4 characters').max(20, 'Lastname must be at most 20 characters'),
            username: z.string().min(4, 'Username must be at least 4 characters').max(20, 'Username must be at most 20 characters'),
            password: z.string().min(8, 'Password must be at least 8 characters').max(16, 'Password must be at most 16 characters').regex(/^(?=.*[A-Z])(?=.*[0-9])[A-Za-z0-9]{8,}$/, 'Password must include at least uppercase 1 letter and 1 digit'),
            repeatedPassword: z.string().min(8, 'Password must be at least 8 characters').max(16, 'Password must be at most 16 characters').regex(/^(?=.*[A-Z])(?=.*[0-9])[A-Za-z0-9]{8,}$/, 'Repeated password must include at least 1 uppercase letter and 1 digit')
        });

        User.parse(user);
    }

    const signUp = async () => {
        let userCredentials :UserCredential;
        try {
            validate({
                    email: `${email}`,
                    firstname: `${firstName}`,
                    lastname: `${lastName}`,
                    username: `${userName}`,
                    password: `${password}`,
                    repeatedPassword: `${repeatedPassword}`
                });

            if (password != repeatedPassword) {
                alert("Passwords don't match");
                return;
            }

            userCredentials = await createUserWithEmailAndPassword(auth, email, password);
            const token = await userCredentials.user.getIdToken();

            const response = await fetch(config.SIGN_UP_URL, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    email: `${email}`,
                    firstname: `${firstName}`,
                    lastname: `${lastName}`,
                    username: `${userName}`,
                })
            });

            if (response.ok) {
                alert('Everything succeeded');
                navigate('/login');
            } else {
                alert('Something went wrong');
                await deleteUser(userCredentials.user);
            }
        } catch (error) {
            if (error instanceof Error) {
                alert("Error: " + error.message);
            } else {
                alert("Unknown error");
            }
            await deleteUser(userCredentials.user);
        }
    };
    return (
        <div style={{backgroundImage: `url(${bgImage})`, backgroundRepeat: 'space repeat'}} className="w-screen h-screen overflow-hidden">
            <div className="h-screen w-1/3 bg-white m-auto">
                <div>
                    <label className="block m-2 font-semibold">Email</label>
                    <input placeholder="Enter your email" type="email" value={email} className="w-full border-2 p-1 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setEmail(e.target.value)}}></input>

                    <label className="block m-2 font-semibold">UserName</label>
                    <input placeholder="Enter your user name" type="email" value={userName} className="w-full border-2 p-1 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setUserName(e.target.value)}}></input>

                    <label className="block m-2 font-semibold">FirstName</label>
                    <input placeholder="Enter your firstName" type="email" value={firstName} className="w-full border-2 p-1 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setFirstName(e.target.value)}}></input>

                    <label className="block m-2 font-semibold">LastName</label>
                    <input placeholder="Enter your lastName" type="email" value={lastName} className="w-full border-2 p-1 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setLastName(e.target.value)}}></input>

                    <label className="block m-2 font-semibold">Password</label>
                    <input placeholder="Enter your password" type="password" value={password} className="w-full p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setPassword(e.target.value)}}></input>

                    <label className="block m-2 font-semibold">Repeat password</label>
                    <input placeholder="Repeat your password" type="password" value={repeatedPassword} className="w-full p-1 border-2 rounded-md border-gray-300 h-10 m-2" onChange={(e) => {setRepeatedPassword(e.target.value)}}></input>

                    <div className="flex justify-between">
                        <button className="m-2 w-25 border-2 border-gray-300" onClick={signUp}>Sign Up</button>
                        <button className="w-48 m-2 border-2 border-gray-300 justify-end inline-block" onClick={LogInWithGoogle}>Log In with Google</button>
                    </div>
                </div>
                <p className="font-bold m-2 text-right">Already have an account?</p>
                <div className="flex justify-end m-2 mt-4">
                    <button className="w-24 border-2 border-gray-300" onClick={goToLogIn}>Log In</button>
                </div>
            </div>
        </div>);
}

export default SignUpPage;