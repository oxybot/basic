import { lazy, Suspense } from "react";
import { useSelector } from "react-redux";
import { authenticationState, SignIn } from "./Authentication";

const AppConnected = lazy(() => import("./AppConnected"));

export default function App() {
  const { connected } = useSelector(authenticationState);

  return (
    <Suspense fallback={<div>loading</div>}>
      {connected && <AppConnected />}
      {!connected && <SignIn />}
    </Suspense>
  );
}
