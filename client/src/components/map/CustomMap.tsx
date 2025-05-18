import React from 'react';
import { MapContainer, TileLayer, Marker, Popup, Circle } from 'react-leaflet';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import {Box} from "@mui/material";

// Координати для центру карти (приблизно на Англію)
const center = [51.5074, -0.1278]; // Лондон

// Список позначок і їх даних (координати, радіуси)
const markersData = [
    { lat: 51.5074, lng: -0.1278, title: 'London', radius: 5000 },
    { lat: 53.4808, lng: -2.2426, title: 'Manchester', radius: 3000 },
    { lat: 52.4862, lng: -1.8904, title: 'Birmingham', radius: 4000 },
];

const CustomMap: React.FC = () => {
    return (
        <Box style={{marginBottom: '40px'}}>
            <MapContainer center={center} zoom={6} style={{ height: '400px', width: '100%'}}>
                <TileLayer
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                    attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                />

                {markersData.map((marker, index) => (
                    <React.Fragment key={index}>
                        <Marker position={[marker.lat, marker.lng]}>
                            <Popup>{marker.title}</Popup>
                        </Marker>
                        <Circle
                            center={[marker.lat, marker.lng]}
                            radius={marker.radius}
                            color="blue"
                            fillColor="blue"
                            fillOpacity={0.2}
                        />
                    </React.Fragment>
                ))}
            </MapContainer>
        </Box>

    );
};

export default CustomMap;
