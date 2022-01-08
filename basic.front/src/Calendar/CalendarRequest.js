import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";

export function CalendarRequest() {
  const definition = useDefinition("EventForEdit");
  const texts = {
    title: "Events",
    subTitle: "Request a new Event",
  };

  return <PageNew definition={definition} baseApiUrl="Events" texts={texts} />;
}
