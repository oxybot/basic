import { Routes, Route } from "react-router-dom";
import Dashboard from "./Dashboard";
import Clients from "./Clients/Clients";
import Client from "./Clients/Client";
import ClientEdit from "./Clients/ClientEdit";
import ClientNew from "./Clients/ClientNew";
import Layout from "./Layout";
import Agreement from "./Agreements/Agreement";
import Agreements from "./Agreements/Agreements";

export default function App() {
  return (
    <Layout>
      <Routes>
        <Route path="/" element={<Dashboard />} />
        <Route path="/clients" element={<Clients />}>
          <Route path=":clientId" element={<Client />} />
          <Route path=":clientId/edit" element={<ClientEdit />} />
        </Route>
        <Route path="/clients/new" element={<ClientNew />} />
        <Route path="/agreements" element={<Agreements />}>
          <Route path=":agreementId" element={<Agreement />} />
        </Route>
      </Routes>
    </Layout>
  );
}
