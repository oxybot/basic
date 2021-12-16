import { Routes, Route } from "react-router-dom";
import Dashboard from "./Dashboard";
import Clients from "./Clients";
import Client from "./Client";
import ClientNew from "./ClientNew";
import Layout from "./Layout";

function App() {
    return (
        <Layout>
            <Routes>
                <Route path="/" element={<Dashboard />} />
                <Route path="/clients" element={<Clients />}>
                    <Route path=":clientId" element={<Client />} />
                </Route>
                <Route path="/clients/new" element={<ClientNew />} />
            </Routes>
        </Layout>
    );
}

export default App;
