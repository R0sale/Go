import React from 'react';
import { useState } from 'react';
import { MapPin, Search, Share2, ShoppingCart, Bed, Plus, Coffee, ShoppingBag, Dumbbell, CreditCard, Fuel, Wrench } from 'lucide-react';
import './index.css';

const categories = [
  { label: 'Where to eat', icon: <ShoppingCart className="w-5 h-5" /> },
  { label: 'Products', icon: <ShoppingBag className="w-5 h-5" /> },
  { label: 'Hotels', icon: <Bed className="w-5 h-5" /> },
  { label: 'Pharmacy', icon: <Plus className="w-5 h-5" /> },
  { label: 'Cafe', icon: <Coffee className="w-5 h-5" /> },
  { label: 'Shopping malls', icon: <ShoppingBag className="w-5 h-5" /> },
  { label: 'Sport', icon: <Dumbbell className="w-5 h-5" /> },
  { label: 'ATM', icon: <CreditCard className="w-5 h-5" /> },
  { label: 'Fuel', icon: <Fuel className="w-5 h-5" /> },
  { label: 'AutoServices', icon: <Wrench className="w-5 h-5" /> },
];

interface SideBarProps {
    onSearch: (coords: [number, number]) => void;
}

const Sidebar: React.FC<SideBarProps> = ({onSearch}) => {
    const [query, setQuery] = useState('');

    const handleSearch = async () => {
        const res = await fetch(`https://nominatim.openstreetmap.org/search?format=json&q=${query}`);
        const data = await res.json();
        if (data.length > 0)
        {
            const { lat, lon } = data[0];
            onSearch([parseFloat(lat), parseFloat(lon)]);
        }
    }

    return (
        <div className="w-full max-w-sm bg-white p-4 rounded-xl shadow-md space-y-4">
        <div className="flex items-center gap-2 bg-gray-100 rounded-full px-4 py-2 shadow-sm">
            <MapPin className="w-5 h-5 text-red-500" />
            <input
            type="text"
            value={query}
            placeholder="Search..."
            onChange={(e) => {setQuery(e.target.value)}}
            className="bg-transparent flex-1 outline-none text-sm"
            />
            <Search onClick={handleSearch} className="w-5 h-5 text-gray-500 hover:bg-gray-200 cursor-pointer" />
            <Share2 className="w-5 h-5 text-gray-500" />
        </div>

        <div className="flex items-center justify-between">
            <div className="text-lg font-semibold">City <span className="text-sm">☀️ ??°C</span></div>
            <button className="text-blue-500 text-sm">All places</button>
        </div>

        <div className="grid grid-cols-5 gap-3 text-center text-xs">
            {categories.map((cat, i) => (
            <div key={i} className="flex flex-col items-center justify-center bg-gray-100 rounded-full p-2 hover:bg-gray-200 cursor-pointer">
                {cat.icon}
                <span className="mt-1">{cat.label}</span>
            </div>
            ))}
        </div>
        </div>
    );
};

export default Sidebar;
