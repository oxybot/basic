import { useSelector } from "react-redux";
import { useDefinition } from "../api";
import { authenticationState } from "../Authentication";
import { disconnect } from "../Authentication";
import PageView from "../Generic/PageView";



export function Logout() {
    const { user } = useSelector(authenticationState);
    const definition = useDefinition("UserForView");

    return (
      <PageView full={true} entity={user}>
disconnect();
      </PageView>
    );
  }