import { Routes, Route } from "react-router-dom";
import { AgreementEdit, AgreementList, AgreementNew, AgreementView } from "./Agreements";
import { usePersistedAuthentication } from "./Authentication";
import { BalanceEdit, BalanceList, BalanceNew } from "./Balances";
import { Calendar, CalendarRequest } from "./Calendar";
import { ClientEdit, ClientList, ClientNew, ClientView } from "./Clients";
import { Dashboard } from "./Dashboard";
import { EventList, EventNew, EventView } from "./Events";
import { EventCategoryEdit, EventCategoryList, EventCategoryNew } from "./EventCategories";
import { GlobalDayOffEdit, GlobalDayOffList, GlobalDayOffNew } from "./GlobalDaysOff";
import Layout from "./Layout";
import { MyEventList, MyEventView, MyPasswordEdit, ProfileEdit, ProfileView } from "./My";
import { ProductEdit, ProductList, ProductNew, ProductView } from "./Products";
import { ScheduleEdit, ScheduleList, ScheduleNew, ScheduleView } from "./Schedules";
import Settings from "./Settings";
import { UserEdit, UserImport, UserList, UserNew, UserView } from "./Users";

export default function App() {
  usePersistedAuthentication();
  return (
    <Layout>
      <Routes>
        {/* Dashboard */}
        <Route path="/" element={<Dashboard />} />

        {/* Calendar */}
        <Route path="/calendar">
          <Route index element={<Calendar />} />
          <Route path="request" element={<CalendarRequest full />} />
        </Route>

        {/* Settings */}
        <Route path="/settings">
          <Route index element={<Settings />} />
          <Route path="event-categories" element={<EventCategoryList />}>
            <Route path=":categoryId" element={<EventCategoryEdit />} />
            <Route path="new" element={<EventCategoryNew />} />
          </Route>
          <Route path="days-off" element={<GlobalDayOffList />}>
            <Route path=":dayOffId" element={<GlobalDayOffEdit />} />
            <Route path="new" element={<GlobalDayOffNew />} />
          </Route>
        </Route>

        {/* Profile */}
        <Route path="/my/profile" element={<ProfileView full />} />
        <Route path="/my/profile/edit" element={<ProfileEdit full />} />
        <Route path="/my/profile/password" element={<MyPasswordEdit full />} />
        <Route path="/my/events" element={<MyEventList />}>
          <Route path=":eventId" element={<MyEventView backTo="/my/events" />} />
          <Route path="new" element={<CalendarRequest />} />
        </Route>

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
          <Route path="import" element={<UserImport />} />
        </Route>

        {/* Balances */}
        <Route path="balances" element={<BalanceList />}>
          <Route path=":balanceId" element={<BalanceEdit />} />
          <Route path="new" element={<BalanceNew />} />
        </Route>

        {/* Schedules */}
        <Route path="/schedules" element={<ScheduleList />}>
          <Route path=":scheduleId" element={<ScheduleView backTo="/schedules" />} />
          <Route path=":scheduleId/edit" element={<ScheduleEdit />} />
          <Route path="new" element={<ScheduleNew />} />
        </Route>

        {/* Events */}
        <Route path="/event/:eventId" element={<EventView full />} />
        <Route path="/events" element={<EventList />}>
          <Route path=":eventId" element={<EventView backTo="/events" />} />
          <Route path="new" element={<EventNew />} />
        </Route>
      </Routes>
    </Layout>
  );
}
