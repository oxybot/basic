import { useSelector } from "react-redux";
import { useDefinition } from "../api";
import { authenticationState } from "../Authentication";
import EntityDetail from "../Generic/EntityDetail";
import PageView from "../Generic/PageView";
import Section from "../Generic/Section";
import Sections from "../Generic/Sections";

export function ProfileView() {
  const { user } = useSelector(authenticationState);
  const definition = useDefinition("MyUserForView");

  return (
    <PageView full={true} entity={user}>
      <Sections>
        <Section code="detail" element={<EntityDetail definition={definition} entity={user} />}>
          Detail
        </Section>
      </Sections>
    </PageView>
  );
}
