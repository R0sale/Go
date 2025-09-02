import React, { useEffect } from 'react';
import type { Dispatch, SetStateAction } from 'react';
import { useState } from 'react';
import '../index.css';
import SearchBar from './SearchBar';
import Categories from './Categories';
import Menu from './Menu';
import getPosition from '../GetPosition';
import type { Facility } from '../models/Facility';
import type { LatLngExpression } from 'leaflet';
import Transport from './Transport';
import type { Vehicle } from '../models/Transport';

interface SideBarProps {
    onSearch: Dispatch<SetStateAction<LatLngExpression>>;
    isDimmed: boolean;
    setFacilities: Dispatch<SetStateAction<Facility[]>>;
    setTransport: Dispatch<SetStateAction<Vehicle[]>>;
    map: L.Map | null;
}

const Sidebar: React.FC<SideBarProps> = ({onSearch, isDimmed, setFacilities, setTransport, map}) => {
    const [isVisible, setIsVisible] = useState(true);
    const [city, setCity] = useState('');

    const handleVisible = () => {
        setIsVisible(false);
    }

    useEffect(() => {
        const fun = async () => {
            const {latitude, longitude} = (await getPosition()).coords;

            const cityUrl = `https://nominatim.openstreetmap.org/reverse?format=json&lat=${latitude}&lon=${longitude}&accept-language=en`;

            const resCity = await fetch(cityUrl);
            const data = await resCity.json();
            setCity(data.address.city);
        }

        fun();
    }, []);

    return (
        <div className="w-200 max-w-md bg-white rounded-xl shadow-md space-y-4">
            <div className='p-4'>
                <SearchBar onSearch={onSearch}/>
            </div>

            {!isDimmed ? (
            <div>
                <div className="flex items-center justify-between h-10 p-4">
                    <div className="text-lg font-semibold">{city}<span className="text-sm">☀️ °C</span></div>
                    {isVisible && <button onClick={handleVisible} className="text-blue-500 text-sm">All places</button>}
                </div>

                <Categories isVisible={isVisible} setFacilities={setFacilities} map={map}/>
                <div className="h-2 bg-gray-100 mt-5 m-0 p-0 w-full"></div>
                <Transport setTransport={setTransport} map={map}/>
            </div>
            ) : <Menu menuState={isDimmed}/>}
        </div>
    );
};

export default Sidebar;
