import React from "react";
import { Car, Bike} from 'lucide-react';
import { Icon } from 'lucide-react';
import { motorRacingHelmet } from '@lucide/lab';
import { config } from "../config";
import type { Vehicle } from "../models/Transport";

const transport = [
    {label: 'Car', icon: <Car className="w-5 h-5"/>},
    {label: 'Motorcycle', icon: <Icon iconNode={motorRacingHelmet} className="w-5 h-5"/>},
    {label: 'Bicycle', icon: <Bike className="w-5 h-5"/>},
    {label: 'Scooter', icon: <Car className="w-5 h-5"/>},
]

interface TransportProps {
    setTransport: (transport: Vehicle[]) => void;
    map: L.Map | null;
};

const Transport: React.FC<TransportProps> = ({ setTransport, map }) => {
    const handleClick = (label: string) => {
            fetch(config.GET_TRANSPORT_URL, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    type: label.toLowerCase(),
                })
            }).then(res => {
                console.log(res);
                return res.json();
            }).then(data => {
                console.log(data);
                setTransport(data);
              })
              .catch(err => alert(err.message));
    };

    return (
        <div className="grid grid-cols-4 gap-4 text-center text-xs mt-3 p-4">
            {transport.map((t, i) => 
            <div key={i} onClick={() => {handleClick(t.label)}} className="flex flex-col items-center w-19 h-19 justify-center bg-gray-100 rounded-full p-2 hover:bg-gray-200 cursor-pointer">
                {t.icon}
                <span className="mt-1">{t.label}</span>
            </div>)}
        </div>
    );
}

export default Transport;