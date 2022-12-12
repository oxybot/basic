import { useMemo } from "react";
import { useSearchParams } from "react-router-dom";

export function useSorting() {
  const [searchParams, setSearchParams] = useSearchParams();

  const sorting = useMemo(() => {
    if (searchParams && searchParams.get("o")) {
      const o = searchParams.get("o").split("-");
      return [{ id: o[0], desc: o.length > 1 && o[1] === "desc" }];
    } else {
      return null;
    }
  }, [searchParams]);

  const updateSorting = function (updater) {
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
  };

  return [sorting, updateSorting];
}
