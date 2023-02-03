import { Route, createBrowserRouter, RouterProvider, createRoutesFromElements } from "react-router-dom";
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
import { apiFetch } from "./api";

function loadList(context, request) {
  const url = new URL(request.url);
  const searchParams = url.searchParams;
  const params = new URLSearchParams();

  // Manage sorting
  if (searchParams && searchParams.get("o")) {
    const values = searchParams.get("o").split("-");
    params.set("sortKey", values[0]);
    params.set("sortValue", values[1] === "desc" ? "desc" : "asc");
  }

  // Manage filters
  if (searchParams && searchParams.getAll("f")) {
    const filters = searchParams.getAll("f");
    filters.forEach((f) => {
      params.append("filters", f);
    });
  }

  return apiFetch(`${context}?${params.toString()}`, { method: "GET" });
}

function loadOne(context, entityId) {
  return apiFetch([context, entityId], { method: "GET" });
}

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route path="/" element={<Layout />}>
      {/* Dashboard */}
      <Route path="/" element={<Dashboard />} />

      {/* Calendar */}
      <Route path="/calendar">
        <Route index element={<Calendar />} />
        <Route path="request" element={<CalendarRequest full />} />
      </Route>

      {/* Profile */}
      <Route path="/my/profile" element={<ProfileView full />} />
      <Route path="/my/profile/edit" element={<ProfileEdit full />} loader={() => loadOne("my/user")} />
      <Route path="/my/profile/password" element={<MyPasswordEdit full />} />
      <Route path="/my/events" element={<MyEventList />} loader={({ request }) => loadList("my/events", request)}>
        <Route
          path=":eventId"
          element={<MyEventView backTo="/my/events" />}
          loader={({ params }) => loadOne("my/events", params.eventId)}
        />
        <Route path="new" element={<CalendarRequest />} />
      </Route>

      {/* Settings */}
      <Route path="/settings">
        <Route index element={<Settings />} />
        <Route
          path="event-categories"
          element={<EventCategoryList />}
          loader={({ request }) => loadList("eventcategories", request)}
        >
          <Route
            path=":categoryId"
            element={<EventCategoryEdit />}
            loader={({ params }) => loadOne("eventcategories", params.categoryId)}
          />
          <Route path="new" element={<EventCategoryNew />} />
        </Route>
        <Route
          path="days-off"
          element={<GlobalDayOffList />}
          loader={({ request }) => loadList("globaldaysoff", request)}
        >
          <Route
            path=":dayOffId"
            element={<GlobalDayOffEdit />}
            loader={({ params }) => loadOne("globaldaysoff", params.dayOffId)}
          />
          <Route path="new" element={<GlobalDayOffNew />} />
        </Route>
      </Route>

      {/* Clients */}
      <Route
        path="/client/:clientId"
        element={<ClientView full />}
        loader={({ params }) => loadOne("clients", params.clientId)}
      />
      <Route
        path="/client/:clientId/edit"
        element={<ClientEdit full />}
        loader={({ params }) => loadOne("clients", params.clientId)}
      />
      <Route path="/clients" element={<ClientList />} loader={({ request }) => loadList("clients", request)}>
        <Route
          path=":clientId"
          element={<ClientView backTo="/clients" />}
          loader={({ params }) => loadOne("clients", params.clientId)}
        />
        <Route
          path=":clientId/edit"
          element={<ClientEdit />}
          loader={({ params }) => loadOne("clients", params.clientId)}
        />
        <Route path="new" element={<ClientNew />} />
      </Route>

      {/* Products */}
      <Route
        path="/product/:productId"
        element={<ProductView full />}
        loader={({ params }) => loadOne("products", params.productId)}
      />
      <Route
        path="/product/:productId/edit"
        element={<ProductEdit full />}
        loader={({ params }) => loadOne("products", params.productId)}
      />
      <Route path="/products" element={<ProductList />} loader={({ request }) => loadList("products", request)}>
        <Route
          path=":productId"
          element={<ProductView backTo="/products" />}
          loader={({ params }) => loadOne("products", params.productId)}
        />
        <Route
          path=":productId/edit"
          element={<ProductEdit />}
          loader={({ params }) => loadOne("products", params.productId)}
        />
        <Route path="new" element={<ProductNew />} />
      </Route>

      {/* Agreements */}
      <Route
        path="/agreement/:agreementId"
        element={<AgreementView full />}
        loader={({ params }) => loadOne("agreements", params.agreementId)}
      />
      <Route
        path="/agreement/:agreementId/edit"
        element={<AgreementEdit full />}
        loader={({ params }) => loadOne("agreements", params.agreementId)}
      />
      <Route path="/agreements" element={<AgreementList />} loader={({ request }) => loadList("agreements", request)}>
        <Route
          path=":agreementId"
          element={<AgreementView backTo="/agreements" />}
          loader={({ params }) => loadOne("agreements", params.agreementId)}
        />
        <Route
          path=":agreementId/edit"
          element={<AgreementEdit />}
          loader={({ params }) => loadOne("agreements", params.agreementId)}
        />
        <Route path="new" element={<AgreementNew />} />
      </Route>

      {/* Users */}
      <Route
        path="/user/:userId"
        element={<UserView full />}
        loader={({ params }) => loadOne("users", params.userId)}
      />
      <Route
        path="/user/:userId/edit"
        element={<UserEdit full />}
        loader={({ params }) => loadOne("users", params.userId)}
      />
      <Route path="/users" element={<UserList />} loader={({ request }) => loadList("users", request)}>
        <Route
          path=":userId"
          element={<UserView backTo="/users" />}
          loader={({ params }) => loadOne("users", params.userId)}
        />
        <Route path=":userId/edit" element={<UserEdit />} loader={({ params }) => loadOne("users", params.userId)} />
        <Route path="new" element={<UserNew />} />
        <Route path="import" element={<UserImport />} />
      </Route>

      {/* Balances */}
      <Route path="balances" element={<BalanceList />} loader={({ request }) => loadList("balances", request)}>
        <Route
          path=":balanceId"
          element={<BalanceEdit />}
          loader={({ params }) => loadOne("balances", params.balanceId)}
        />
        <Route path="new" element={<BalanceNew />} />
      </Route>

      {/* Schedules */}
      <Route path="/schedules" element={<ScheduleList />} loader={({ request }) => loadList("schedules", request)}>
        <Route
          path=":scheduleId"
          element={<ScheduleView backTo="/schedules" />}
          loader={({ params }) => loadOne("schedules", params.scheduleId)}
        />
        <Route
          path=":scheduleId/edit"
          element={<ScheduleEdit />}
          loader={({ params }) => loadOne("schedules", params.scheduleId)}
        />
        <Route path="new" element={<ScheduleNew />} />
      </Route>

      {/* Events */}
      <Route
        path="/event/:eventId"
        element={<EventView full />}
        loader={({ params }) => loadOne("events", params.eventId)}
      />
      <Route path="/events" element={<EventList />} loader={({ request }) => loadList("events", request)}>
        <Route
          path=":eventId"
          element={<EventView backTo="/events" />}
          loader={({ params }) => loadOne("events", params.eventId)}
        />
        <Route path="new" element={<EventNew />} />
      </Route>
    </Route>
  )
);

export default function App() {
  usePersistedAuthentication();
  return <RouterProvider router={router} />;
}
