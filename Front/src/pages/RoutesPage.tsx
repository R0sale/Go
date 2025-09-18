import { useEffect, useState } from "react";
import type { Route } from "../models/Route";
import { auth } from "../firebase";
import { config } from "../config";
import { useNavigate } from "react-router-dom";
import RouteMap from "./components/RouteMap";



const RoutesPage: React.FC = () => {
    const [routes, setRoutes] = useState<Route[]>([]);
    const [loading, setLoading] = useState(false);
    const [isChecked, setIsChecked] = useState(false);
    const [selectedRoute, setSelectedRoute] = useState<Route | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        const unsubscribe = auth.onAuthStateChanged(async (user) => {
            if (user) {
                setLoading(true);
                const token = await user.getIdToken(true);

                console.log(token);

                const result = await fetch(`${config.GET_USERS_ROUTES_URL}`, {
                    method: 'GET',
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${token}`
                    }
                });

                const routes = await result.json();
                setRoutes(routes);
                setLoading(false);
            } else {
                alert("Something Went Wrong");
                navigate('/');
            }
        });

        

        return () => unsubscribe();
    }, [navigate]);

    if (loading) {
        return (<div className="flex h-screen w-screen">
            <span className="loader m-auto"></span>
        </div>);
    }


    return (
        <div className="flex overflow-hidden min-h-screen bg-gray-100">
            <div className="mt-4 mb-4 ml-6 mr-6 w-screen bg-white ">
                <h1 className="text-4xl font-bold ml-10 mt-10">Routes Page</h1>
                <div className="flex">
                    <div className="w-1/3 m-10 h-100">
                            <div className="bg-gray-100 rounded-2xl border-2 border-black h-full">
                                <div className="flex justify-between">
                                    <p className="m-2 mt-4 text-2xl w-1/3">Your Routes:</p>
                                </div>
                                <p className="h-1 bg-white m-0 p-0 w-full"></p>
                                <div className="overflow-y-scroll h-80">
                                    {routes.map((route, i) => {
                                        return (
                                            <div key={i} className="p-2 w-full h-15 leading-10.5 font-medium cursor-pointer hover:bg-gray-200" onClick={() => {setSelectedRoute(route); setIsChecked(true)}}>
                                                {route.name}: {route.city}
                                            </div>
                                        );
                                    })}
                                </div>
                            </div>
                            <button className="w-35 h-12 text-center items-center text-l m-2 border-2 border-gray-300" >Create Route</button>
                    </div>
                    {isChecked && <div className="ml-10 mt-10">
                        <p className="text-2xl font-bold mb-4">Route Details:</p>
                        <div className="w-96 h-60 bg-gray-100 rounded-2xl border-2 border-black p-4">
                            <div className="flex h-15 justify-between">
                                <p className="text-xl leading-15 font-semibold">Name:</p>
                                <p className="text-xl leading-15 mr-10">{selectedRoute?.name}</p>
                            </div>
                            <div className="flex h-15 justify-between">
                                <p className="text-xl leading-15 font-semibold">City:</p>
                                <p className="text-xl leading-15 mr-10">{selectedRoute?.city}</p>
                            </div>
                            <div className="flex h-15 justify-between">
                                <p className="text-xl leading-15 font-semibold">Description:</p>
                                <p className="text-xl leading-15 mr-10">{selectedRoute?.description}</p>
                            </div>
                        </div>
                    </div>}
                    <div className="ml-20">
                        <RouteMap coords={ selectedRoute?.nodes.length ? [selectedRoute.nodes[0].latitude, selectedRoute.nodes[0].latitude] : [45.2671, 19.8335]}
                        nodes={selectedRoute ? selectedRoute.nodes : []} />
                    </div>
                </div>
            </div>
        </div>
    );
}

export default RoutesPage;