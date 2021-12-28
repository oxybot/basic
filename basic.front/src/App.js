import { Routes, Route } from "react-router-dom";
import { AgreementEdit, AgreementList, AgreementNew, AgreementView } from "./Agreements";
import { ClientEdit, ClientList, ClientNew, ClientView } from "./Clients";
import Dashboard from "./Dashboard";
import Layout from "./Layout";
import { ProductEdit, ProductList, ProductNew, ProductView } from "./Products";
import { UserEdit, UserList, UserNew, UserView } from "./Users";

export default function App() {
  return (
    <Layout>
      <Routes>
        {/* Dashboard */}
        <Route path="/" element={<Dashboard />} />

        {/* Clients */}
        <Route path="/client/:clientId" element={<ClientView full />} />
        <Route path="/client/:clientId/edit" element={<ClientEdit full />} />
        <Route path="/clients" element={<ClientList />}>
          <Route path=":clientId" element={<ClientView backTo="/clients" />} />
          <Route path=":clientId/edit" element={<ClientEdit />} />
          <Route path="new" element={<ClientNew />} />
        </Route>

        {/* Products */}
        <Route path="/product/:productId" element={<ProductView full />} />
        <Route path="/product/:productId/edit" element={<ProductEdit full />} />
        <Route path="/products" element={<ProductList />}>
          <Route path=":productId" element={<ProductView backTo="/products" />} />
          <Route path=":productId/edit" element={<ProductEdit />} />
          <Route path="new" element={<ProductNew />} />
        </Route>

        {/* Agreements */}
        <Route path="/agreement/:agreementId" element={<AgreementView full />} />
        <Route path="/agreement/:agreementId/edit" element={<AgreementEdit full />} />
        <Route path="/agreements" element={<AgreementList />}>
          <Route path=":agreementId" element={<AgreementView backTo="/agreements" />} />
          <Route path=":agreementId/edit" element={<AgreementEdit />} />
          <Route path="new" element={<AgreementNew />} />
        </Route>

        {/* Users */}
        <Route path="/user/:userId" element={<UserView full />} />
        <Route path="/user/:userId/edit" element={<UserEdit full />} />
        <Route path="/users" element={<UserList />}>
          <Route path=":userId" element={<UserView backTo="/users" />} />
          <Route path=":userId/edit" element={<UserEdit />} />
          <Route path="new" element={<UserNew />} />
        </Route>
      </Routes>
    </Layout>
  );
}
