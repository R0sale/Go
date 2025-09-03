import type React from "react";
import { use, useEffect, useState } from "react";
import { auth } from "../firebase";
import type { TokenResult } from "../models/TokenResult";
import type { User } from "../models/User";
import { config } from "../config";
import { useNavigate } from "react-router-dom";

const AdminPage: React.FC = () => {
    const [tokenResult, setTokenResult] = useState<TokenResult>({
        userName: '',
        firstName: '',
        lastName: '',
        email: '',
        roles: [],
    });
    const navigate = useNavigate();
    const [users, setUsers] = useState<User[]>([]);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
            setLoading(true);
    
            const unsubscribe = auth.onAuthStateChanged(async (user) => {
                if (user) {
                    const token = await user.getIdTokenResult();
                    setTokenResult(token.claims);
                } else {
                    setTokenResult({
                        userName: '',
                        firstName: '',
                        lastName: '',
                        email: '',
                        roles: [],
                    });
                }
    
                setLoading(false);
            });
    
            return () => unsubscribe();
        }, []);

    useEffect(() => {
        const unsubscribe = auth.onAuthStateChanged(async (user) => {
            try {
                setLoading(true);

                if (user) {
                    const token = await user.getIdToken(true);

                const response = await fetch(config.GET_ALL_USERS_URL, {
                    method: 'GET',
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${token}`
                    }
                });

                const result = await response.json();

                setUsers(result);
                } else {
                    alert("Something Went Wrong");
                    navigate('/');
                }
                
                setLoading(false);
            } catch (error) {
                alert(error);
            }
        });

        return () => unsubscribe();
    }, [navigate]);


    if (loading) {
        return (<div className="flex h-screen w-screen">
            <span className="loader m-auto"></span>
        </div>);
    }

    const data = [
        {label: "Username", data: `${tokenResult.userName}`},
        {label: "Full name", data: `${tokenResult.firstName} ${tokenResult.lastName}`},
        {label: "Email", data: `${tokenResult.email}`},
        {label: "Roles", data: `${tokenResult.roles}`}
    ];

    return <div className="h-screen flex pinned-left bg-gray-100 overflow-y-hidden">
        <div className="mt-4 mb-4 ml-6 mr-6 w-screen bg-white ">
            <p className="text-6xl font-bold ml-30 mt-20">Admin Page</p>
            <div className="w-2/3 mt-20">
                        {data.map((row, i) => {
                            return (<div className="ml-4 mt-4 text-xl justify-between flex" key={i}>
                            <p className="p-2">{row.label}</p>
                            <p className="mr-10 w-140 p-2  rounded-2xl border-1 bg-gray-100">{row.data}</p>
                        </div>);
                        })}
                    </div>
            <div className="w-2/3 mt-20 flex">
                <p className="text-3xl font-bold ml-10 mb-10">All Users:</p>
                <div className="overflow-y-scroll h-80 w-160 ml-20">
                    {users.map((user, i) => {
                        return (
                            <div key={i} className="p-2 w-full h-15 leading-10.5 font-medium cursor-pointer hover:bg-gray-200" onClick={() => {navigate(`/adminPage/user/${user.firebaseUid}`)}}>
                                {user.userName}: {user.roles.join(", ")}
                            </div>
                        );
                    })}
                </div>
            </div>
        </div>
    </div>
}

export default AdminPage;