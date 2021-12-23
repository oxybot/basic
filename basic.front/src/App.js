import { Routes, Route } from "react-router-dom";
import Dashboard from "./Dashboard";
import ClientEdit from "./Clients/ClientEdit";
import ClientList from "./Clients/ClientList";
import ClientNew from "./Clients/ClientNew";
import ClientView from "./Clients/ClientView";
import Layout from "./Layout";
import Agreement from "./Agreements/Agreement";
import Agreements from "./Agreements/Agreements";

export default function App() {
  return (
    <Layout>
      <Routes>
        <Route path="/" element={<Dashboard />} />
        <Route path="/client/:clientId" element={<ClientView />} />
        <Route path="/client/:clientId/edit" element={<ClientEdit />} />
        <Route path="/clients" element={<ClientList />}>
          <Route path=":clientId" element={<ClientView backTo="/clients" />} />
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
