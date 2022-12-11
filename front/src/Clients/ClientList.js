import { useMemo } from "react";
import { useLoaderData, useParams, useSearchParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageList from "../Generic/PageList";

export function ClientList() {
  const { clientId } = useParams();
  const definition = useDefinition("ClientForList");
  const texts = {
    title: "Clients",
    add: "Add client",
  };

  const elements = useLoaderData();
  const [searchParams, setSearchParams] = useSearchParams();

  const sorting = useMemo(() => {
    if (searchParams && searchParams.get("o")) {
      const o = searchParams.get("o").split("-");
      const id = o[0];
      const desc = o.length > 1 && o[1] === "desc";
      const s = [{ id, desc }];
      return s;
    }
    return null;
  }, [searchParams]);

  function updateSorting(updater) {
    const entries = Object.fromEntries(searchParams.entries());
    const newSorting = updater(sorting);
    if (newSorting && newSorting.length > 0) {
      const id = newSorting[0].id;
      if (newSorting[0].desc) {
        entries.o = `${id}-desc`;
      } else {
        entries.o = id;
      }
    } else {
      delete entries.o;
    }

    setSearchParams(entries);
  }

  return (
    <PageList
      definition={definition}
      elements={elements}
      selectedId={clientId}
      texts={texts}
      newRole="client"
      sorting={sorting}
      setSorting={updateSorting}
    />
  );
}
