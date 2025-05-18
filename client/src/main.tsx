import { createRoot } from 'react-dom/client'
import './index.css'
import './styles/fonts.css'
import './theme/colors.css'
import AppWrapper from "./utils/wrapper/AppWrapper.tsx";

createRoot(document.getElementById('root')!).render(
    <AppWrapper />
)
