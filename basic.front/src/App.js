import { Routes, Route } from "react-router-dom";
import AgreementEdit from "./Agreements/AgreementEdit";
import AgreementList from "./Agreements/AgreementList";
import AgreementNew from "./Agreements/AgreementNew";
import AgreementView from "./Agreements/AgreementView";
import ClientEdit from "./Clients/ClientEdit";
import ClientList from "./Clients/ClientList";
import ClientNew from "./Clients/ClientNew";
import ClientView from "./Clients/ClientView";
import Dashboard from "./Dashboard";
import Layout from "./Layout";

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

        <Route path="/agreement/:agreementId" element={<AgreementView />} />
        <Route
          path="/agreement/:agreementId/edit"
          element={<AgreementEdit />}
        />
        <Route path="/agreements" element={<AgreementList />}>
          <Route
            path=":agreementId"
            element={<AgreementView backTo="/agreements" />}
          />
          <Route path=":agreementId/edit" element={<AgreementEdit />} />
        </Route>
        <Route path="/agreements/new" element={<AgreementNew />} />
      </Routes>
    </Layout>
  );
}
