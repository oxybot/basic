import { lazy, Suspense } from "react";
import { useSelector } from "react-redux";
import AlertManager from "./Alerts/AlertManager";
import { authenticationState, SignIn } from "./Authentication";

const AppConnected = lazy(() => import("./AppConnected"));

export default function App() {
  const { authenticated } = useSelector(authenticationState);

  console.log(authenticated);
  
  return (
    <Suspense fallback={<div>loading</div>}>
      <AlertManager>
        {authenticated && <AppConnected />}
        {!authenticated && <SignIn />}
      </AlertManager>
    </Suspense>
  );
}
