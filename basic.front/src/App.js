import { Routes, Route } from "react-router-dom";
import Dashboard from "./Dashboard";
import Clients from "./Clients/Clients";
import Client from "./Clients/Client";
import ClientNew from "./Clients/ClientNew";
import Layout from "./Layout";
import ClientContract from "./ClientContracts/ClientContract";
import ClientContracts from "./ClientContracts/ClientContracts";

function App() {
  return (
    <Layout>
      <Routes>
        <Route path="/" element={<Dashboard />} />
        <Route path="/clients" element={<Clients />}>
          <Route path=":clientId" element={<Client />} />
        </Route>
        <Route path="/clients/new" element={<ClientNew />} />
        <Route path="/clientcontracts" element={<ClientContracts />}>
          <Route path=":contractId" element={<ClientContract />} />
        </Route>
      </Routes>
    </Layout>
  );
}

export default App;
