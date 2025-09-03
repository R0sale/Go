import type React from "react";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { auth } from "../firebase";
import { config } from "../config";
import type { User } from "../models/User";
import { sl } from "zod/locales";

const UserManagingPage: React.FC = () => {
    const { userUid } = useParams();
    const [rolesVisible, setRolesVisible] = useState(false);
    const navigate = useNavigate();
    const [user, setUser] = useState<User>({
        firebaseUid: '',
        firstName: '',
        lastName: '',
        userName: '',
        email: '',
        roles: []
    });
    const [loading, setLoading] = useState(false);
    const [selectedRoles, setSelectedRoles] = useState<string[]>([]);
    const [token, setToken] = useState<string>('');

    useEffect(() => {
        setLoading(true);
        const unsubscribe = auth.onAuthStateChanged(async (user) => {
            if (user) {
                const token = await user.getIdToken(true);

                setToken(token);

                const response = await fetch(`${config.GET_USER_BY_UID__URL}${userUid}`, {
                    method: 'GET',
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${token}`
                    }
                });

                const result = await response.json();
                
                setUser(result);
                setSelectedRoles(result.roles);

                setLoading(false);
            } else {
                alert("Something Went Wrong");
                navigate('/');
            }
        });

        return () => unsubscribe();
    }, [navigate, userUid]);

    const handleRoleSelection = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { value, checked } = e.target;

        if (checked) {
            setSelectedRoles(prev => [...prev, value]);
        } else {
            setSelectedRoles(prev => prev.filter(role => role !== value));
        }
    }

    if (loading) {
        return (<div className="flex h-screen w-screen">
            <span className="loader m-auto"></span>
        </div>);
    }

    const userData = [
        { label: 'First Name', value: user.firstName },
        { label: 'Last Name', value: user.lastName },
        { label: 'User Name', value: user.userName },
        { label: 'Email', value: user.email }
    ]

    const roles = [
        {name: 'Admin', id: 1},
        {name: 'TransportManager', id: 2},
        {name: 'RouteManager', id: 3},
        {name: 'User', id: 4},
    ]

    const handleSubmit = async () => {
        try {
            const result = await fetch(`${config.UPDATE_USER_ROLES_URL}${userUid}`, {
                method: 'PUT',
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                },
                body: JSON.stringify(selectedRoles)
            });

            console.log(result.ok);
        } catch (error) {
            alert(error);
        }
    }
    


    return <div className="flex h-screen bg-gray-100">
        <div className="mt-4 mb-4 ml-6 mr-6 w-screen bg-white ">
            <div>
                <p className="text-3xl font-bold ml-10 mb-10 mt-10">User Details:</p>
                <div className="ml-10">
                    {userData.map((data, i) => (<div className="ml-4 mt-4 text-xl justify-between flex">
                            <p className="p-2 font-semibold">{data.label}</p>
                            <p className="mr-40 w-140 p-2  rounded-2xl border-1 bg-gray-100">{data.value}</p>
                    </div>))}
                    <div className="flex">
                        <p className="text-xl font-semibold h-10 p-2 m-3">Roles: {user?.roles.join(", ")}</p>
                        <button className="w-52 h-13 text-center mt-2 items-center text-xl flex border-2 border-gray-300" onClick={() => {setRolesVisible(!rolesVisible)}}>Rearrange Roles</button>
                        {rolesVisible && 
                        <form className="w-45 h-35 bg-gray-200 ml-10 rounded-2xl mt-2" onSubmit={async (e) =>  { e.preventDefault(); await handleSubmit();}} >
                            {roles.map((role, i) => (<div>
                                <label className="ml-4 mt-2 flex" key={i}>
                                <input type="checkbox" className="" key={i} name={role.name} value={role.name} checked={selectedRoles.includes(role.name)} onChange={e => handleRoleSelection(e)} /> 
                                <span className="ml-2">{role.name}</span>
                                    </label>
                            </div>))
                            }
                            <input type="submit" className="w-20 h-8 bg-blue-500 text-white rounded-2xl mt-4 " value="Submit" />
                        </form>}
                    </div>
                </div>
            </div>
        </div>
    </div>
}

export default UserManagingPage;