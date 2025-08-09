import React, { useState } from "react";
import { createUserWithEmailAndPassword, deleteUser, type UserCredential } from "firebase/auth";
import { auth } from "../firebase";
import bgImage from '../assets/worldmap.jpg';
import { useNavigate } from "react-router-dom";

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

    const login = async () => {
        let userCredentials :UserCredential;

        try {
            userCredentials = await createUserWithEmailAndPassword(auth, email, password);
            const token = await userCredentials.user.getIdToken();

            const response = await fetch('https://localhost:7023/api/users', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    email: `${email}`,
                    password: `${password}`,
                    firstname: `${firstName}`,
                    lastname: `${lastName}`,
                    username: `${userName}`
                })
            });

            if (response.ok) {
                alert('Everything succeeded');
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
        <div style={{backgroundImage: `url(${bgImage})`, backgroundRepeat: 'space repeat'}} className="w-screen h-screen">
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

                    <button className="m-2 w-25 border-2 border-gray-300" onClick={login}>Sign Up</button>
                </div>
                <p className="font-bold m-2 text-right">Already have an account?</p>
                <div className="flex justify-end m-2 mt-4">
                    <button className="w-24 border-2 border-gray-300" onClick={goToLogIn}>Log In</button>
                </div>
            </div>
        </div>);
}

export default SignUpPage;